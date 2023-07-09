1. Опис на игра:
 - Stack Attack е игра која го предизвикува играчот брзо да размислува и да направи добро пресметани потези со цел да добие повеќе поени и да ја совлада механиката на играта. Ова претставува рекреација на познатата игра на старите телефони Siemens и оригиналната игра може да ја погледнете [тука](https://www.youtube.com/watch?v=cjh7mMdTulk).
- При стартување на играта се појавува прозорец (Слика 1) каде што има 3 опции:
   - Со првата опција "Play" играта започнува. Но најпрво пожелно е играчот да го внесе своето име во полето "Your name..." со цел подоцна да бидат зачувани податоците за неговите поени.
   - Со притискање на "Difficulty" се отвара нов прозорец каде што играчот може да ја избере тежината на играта. Има 3 опции: Easy, Medium и Hard. (Слика 2)
   - И со избирање на последната опција "Exit" се затвара играта.





 - Играта започнува со неколку случајно поставени кутии (Слика 3). Кутиите се спуштаат од горниот дел на екранот и откако ќе стигнат до подот играчот ќе може да ги поместува налево или надесно. Има и една дополнителна црна кутија која не може да се поместува. Целта е да се направи целосно пополнет ред од кутии кои, слично како кај играта тетрис, ќе се уништи редот и со тоа играчот добива поени коишто се прикажани во горниот десен агол на прозорецот. 100 поени се добиваат со секоја скршена кутија, а 200 поени за секоја кутија ако се скрши цел ред.
     
- Освен кутии на подот може да се спуштат и други објекти „power-ups“, коишто на играчот му даваат привремени моќи. Синиот power up (Слика 4) го забрзува движењето на играчот, а зелениот (Слика 6) му овозможува да скокне повисоко. Времетраењето на двата е 10 секунди. Последниот помошник е бомбата (Слика 5) со којашто играчот може да ги уништи околните кутии и за истите да добие поени.
     
 - Додека паѓаат кутиите играчот има опција да скокне кон нив и да ги уништи пред да дојдат до земјата. Играта завршува кога на главата на играчот ќе падне кутија, или една колона ќе биде целосно пополнета со кутии и со тоа се појавува прозорецот на којшто пишува „Game Over“ (Слика 7). Тука играчот може да види колку пени освоил.
Во „Leaderboard“ може да се видат поените од претходно изиграните игри подредени според бројот на освоените поени (Слика 8). Овие резултати се зачувуваат долку играчот притисне на копчето „Еxit“. 
Играчот потоа има опција да започне нова игра со притискање на „Play again“ или пак целосно да ја затвори играта со „Exit“.

2. Опис на решението:
-	Сите потребни скрипти може да се пронајдат во фолдерот “Assets/Scripts”. Сите фајлови со .cs се скриптите, додека фалјовите со .cs.meta се креирани автоматски од Unity
 -	Сите функции се опишани во скриптите со соодветни коментари
  -	Играта е креирана со 11 скрипти од кои што најважната е скриптата Scene.
 -	Кутиите се чуваат во дводимензионална листа која што се пополнува со една кутија кога таа ќе престане да се движи. Листата го претставува координатниот систем на играта каде што кутијата Boxes[0][1] припаѓа на првата колона и на вториот ред. Го одбравме овој начин на чување на кутиите бидејќи вака најлесно може да се креира играта. Доколку ни е потребен следниот ред каде што може да се смести кутија, можеме само да повикаме Boxes[column].Count
 -	Сите колизии се направени со системот на колизии на Unity, каде што се користат Triggers за детекција дека некој објект е во внатрешноста на друг објект.
 -	Зачувувањето на играчите и нивните највисоки поени е направено со серијализација на играчите во фалј кој што се наоѓа во фолдерот “Stack Attack_Data” на играта, а во фолдерот “Assets” на проектот.

3. Опис на главните функции:
- Функција за генерирање кутија: `GenerateBox()` во скриптата Scene.cs
    - Кутиите автоматски се генерираат на одредено време со random боја и колона, а за црната кутија и сите “power-ups” се користи random број за да се добие посакуваната шанса да се појави одредениот објект.
    - Променливата side претставува страната од која што ќе се појави кутијата (0-лево, 1-десно).
    - Променливата ChanceForBlack и ChanceForPowerUp претставуваат процентите на кои што има шанса да се појави објектот. Овие се променуваат во зависност од тежината на играта.
    - Со функцијата `Instantiate()` креираме клон од од кутијата. Оваа функција има потреба од три аргументи(референца до објектот, позиција, ротација).
   - Преку функцијата `GetComponent<>()` пристапуваме до одредената компонента на објектот, во овој случај скриптата на кутијата. Скриптата ни е потребна за да ги иницијализираме потребните фредности на функцијата.
    
 - Функцијата за движење на кутија: `Move()` во скриптата `BoxScript.cs`
     - Функцијата `Move()` прима еден аргумент, страната на која треба да се подвижи.
     - Доколку кутијата не е црна, се проверуваат неколку работи:
       - Дали кутијата не е на крај на екранот.
       - Дали оваа кутија е последната кутија која не се движи во колоната.
       - Дали има простор во наредната колона и на која страна ја движиме.
   - Доколку се исполнат условите, кутијата се поместува во одредениот правец:
     - Се трга од матрицата на кутии.
     - Се намалува или зголемува колоната.
     - Се ресетираат логичките променливи потребни за кутијата повторно да се движи.
   - Функцијата се повикува од страна на играчот кога ќе се придвижи и ја допира кутијата во скриптите `BoxMoverLeftScript.cs` и `BoxMoverRightScript.cs`




    
   


