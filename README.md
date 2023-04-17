**IoT Academy praktikos atrankos darbo eiga**

**1 Užduotis**
Pirmoji užduotis reikalavo nuskaityti duomenis iš JSON failo. Tai padariau nenaudojant NuGet paketų, o sekdamas dokumentaciją apie Built-in standartinę System.Text.Json biblioteką šioje nuorodoje: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0#how-to-read-json-as-net-objects-deserialize 

**2 Užduotis**
 Antroji užduotis buvo panaši kaip pirmoji, tačiau duomenys buvo CSV faile. Nusprendžiau sukurti funkciją kuri skaityto duomenis po vieną String eilutę, su Split funkcija ją išskaido vietose kuriose randa kablelį ir išskaidytus String tipo duomenis Parsina į atitinkamus duomenų tipus, kuriuos saugojau klasės objekte. Tuomet objektus pridėjau prie klasės sąrašo, kurį gale funkcijos ir gražinau.

**3 ir 4 Užduotys**
Trečioji ir ketvirtoji užduotis reikalavo sukurti histogramas GPS duomenų greičiui ir palydovų kiekiui, kadangi tiek greitis tiek palydovai yra INT tipo duomenys, nusprendžiau sukurti funkciją, kuri apskaičiuoja histogramos duomenis(Šią funkciją naudosim abiem histogramoms). Bekurdamas histogramų vizualizaciją pamačiau, jog su greičio duomenimis histograma gali būti labai didelė, todėl sukūriau dvi procedūras, jog spausdintų histogramą. Viena procedūra spausdina mažas histogramas, turinčias mažai duomenų. Kita histograma spausdina duomenų intervalus, panašius, kurie yra nurodyti užduoties aprašyme. Tokiu būdu programa dabar gali spausdinti bet kokią histogramą ir pagal duomenų dydį, juos atvaizduoti arba per didelę arba per mažą histogramą.

**5 Užduotis**
	Penktoje užduotyje man reikėjo rasti greičiausią laiką per kurį buvo nuvažiuota bent 100 km. Kadangi problemai išspresti reikėjo skaičiuoti atstumus tarp taškų ir daugelis jų kartojosi per iteracijas pasirinkau užduočiai atlikti  „Sliding Window“ algoritmą jog sudėtingumas būtų O(N), vietoj O(N*k), tokiu būdu programa užtruks mažiau laiko. Implementavus algoritmą tereikėjo sukurti rezultatų spausdinimą ir užduotis buvo baigta.

**Bonus Užduotis**
	Bonus užduotis buvo panaši kaip pirmoji ir antroji, tačiau duomenis reikėjo išgauti iš BIN failo. Betestuojant skirtingus metodus radau, jog vienas dailesnių sprendimų buvo nusiskaityti duomenis į specialiai sukurtus Byte masyvus, kurių nuskaitytą informaciją vėliau konvertavau į INT formatus, o vėliau Castinau į galutinius formatus. 
