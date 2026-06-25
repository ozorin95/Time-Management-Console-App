# Time-Management-Console-App
C# .NET console application for time management, featuring CSV file handling and input validation.

Aplikace slouží ke sledování a analýze denního času uživatele přes interaktivní textové menu v konzoli. Umožňuje uživateli zadávat vlastní aktivity, řadit je do dynamických kategorií (např. Škola, Sport, Spánek) a zaznamenávat strávený čas. Data jsou perzistentně ukládána do CSV souboru. Program následně provádí analýzu dne, vizualizuje využití času a pomocí lingvistické analýzy klíčových slov poskytuje uživateli chytrá doporučení (např. varování při nedostatku spánku nebo přílišném čase před obrazovkou). 

Aplikace klade velký důraz na uživatelský zážitek (animace, barvy) a bezpečné ošetření chyb. 

Návrh hlavních proměnných a datových struktur: 

• totals – proměnná typu Dictionary<string, double> uchovávající názvy kategorií jako klíče a jejich celkový součet hodin jako hodnoty. 

• details – proměnná typu Dictionary<string, List> uchovávající historii konkrétních aktivit (seznam textů) pro každou danou kategorii. 

• activitiesFile – proměnná typu string obsahující dynamicky zjištěnou absolutní cestu k CSV souboru. 

• records – proměnná typu string[] (pole řetězců) načítající všechny řádky z CSV souboru pro účely analýzy. 

Koncepční popis programu: 

Program spustí úvodní načítací animaci a vyhodnotí cestu pro ukládání dat. 

Zobrazí se hlavní barevné menu a program čeká na uživatelský vstup. 

Podle volby program provede příslušnou akci: 
o Přidá novou aktivitu (s validací zadaných hodin) a uloží data do CSV souboru.
o Zobrazí denní analýzu: agreguje data z CSV přes slovníky (Dictionary), vykreslí využití času a vygeneruje AI-like rady na základě hledání klíčových slov v kategoriích.
o Bezpečně resetuje všechna data po dodatečném potvrzení uživatelem.o Ukončí program s plynulou vypínací animací. 

Přidávání dat probíhá v cyklu, dokud se uživatel nerozhodne vrátit do menu prázdným stiskem klávesy Enter. 

Vstupní omezení a možné problémy: 

• Hodiny strávené aktivitou musí být platné číslo v rozmezí 0.1 až 24. 

• Textové vstupy (název aktivity, kategorie) jsou ošetřeny proti zbytečným mezerám na okrajích pomocí metody Trim(). 

• Součet zaznamenaných hodin za den nesmí přesáhnout 24 hodin. 
