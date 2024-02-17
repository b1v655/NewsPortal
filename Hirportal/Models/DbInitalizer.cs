using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hirportal.Models
{
    public static class DbInitalizer
    {
        private static HirportalContext context;
        private static UserManager<User> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;
        public static void Initialize(HirportalContext ccontext, 
			UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, string imageDirectory)
        {
            context = ccontext;
            _userManager = userManager;
            _roleManager = roleManager;

            context.Database.EnsureCreated();
            if (context.Articles.Any())
            {
                System.Diagnostics.Debug.WriteLine("nem");
                return; 
            }
            SeedUsers();
            SeedArticles();
            SeedImages(imageDirectory);
           
        }
        private static void SeedArticles()
        {
            var Articles = new Article[]
           {
                new Article
                {

                    UserId=1,
                    Title="In vino veritas",
                    Summary="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla aliquet, nibh et consectetur ullamcorper, ligula nunc pellentesque erat, eget faucibus massa orci quis neque. Suspendisse porta turpis nec sem commodo viverra ut ac urna. Aenean cursus commodo eros ut tincidunt. Aliquam quis elementum nisl, at vehicula lorem. Ut imperdiet magna dui, vel pharetra ex consectetur a. Duis facilisis dui ac elit vulputate, ac posuere velit molestie. Vivamus pulvinar fermentum neque, in lacinia arcu hendrerit quis. Etiam sed auctor tortor. Quisque eget mauris ac orci semper lacinia. Nunc elit massa, venenatis congue nulla nec, laoreet bibendum elit. Aenean tristique, dui ut luctus consequat, neque enim malesuada dui, sed cursus sem ipsum at velit. Sed eu ex mi. Vivamus elementum luctus dui eu ultricies. Suspendisse fringilla sem id vulputate vulputate. Curabitur turpis neque, efficitur non ante et, sollicitudin sagittis turpis. Integer molestie lacus nisi, eu consequat odio porta egestas.",
                    Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla aliquet, nibh et consectetur ullamcorper, ligula nunc pellentesque erat, eget faucibus massa orci quis neque. Suspendisse porta turpis nec sem commodo viverra ut ac urna. Aenean cursus commodo eros ut tincidunt. Aliquam quis elementum nisl, at vehicula lorem. Ut imperdiet magna dui, vel pharetra ex consectetur a. Duis facilisis dui ac elit vulputate, ac posuere velit molestie. Vivamus pulvinar fermentum neque, in lacinia arcu hendrerit quis. Etiam sed auctor tortor. Quisque eget mauris ac orci semper lacinia. Nunc elit massa, venenatis congue nulla nec, laoreet bibendum elit. Aenean tristique, dui ut luctus consequat, neque enim malesuada dui, sed cursus sem ipsum at velit. Sed eu ex mi. Vivamus elementum luctus dui eu ultricies. Suspendisse fringilla sem id vulputate vulputate. Curabitur turpis neque, efficitur non ante et, sollicitudin sagittis turpis. Integer molestie lacus nisi, eu consequat odio porta egestas.Cras semper urna eu est scelerisque, porta malesuada turpis viverra. Proin nulla felis, gravida in fermentum ut, elementum imperdiet dolor. Vestibulum nec purus sit amet neque dictum porta in id mi. Pellentesque quis erat euismod, commodo dolor nec, auctor quam. Praesent sed risus quis urna faucibus iaculis in at mauris. Mauris in dapibus quam, ac aliquam est. In eget sapien ac tortor lobortis tristique elementum eu erat. Ut a lorem sit amet velit egestas lacinia eget sed turpis. Nunc ullamcorper risus neque, et facilisis tellus dapibus sed. Ut sed ante ligula. Sed placerat nunc risus, vel interdum nisi finibus ut. Etiam consectetur pretium consequat. Duis euismod sit amet odio ac pharetra. Morbi in porta augue. Morbi vitae vestibulum dolor. Integer ultrices mi magna, id ultricies lectus luctus ac.",
                    Date=new DateTime(2019,2,11,12,25,14),
                    IsMainArticle=true
                },
                /*new Article
                {

                    UserId=2,
                    Title="Презря скрыты Пускай кроток океане завидь",
                    Summary="Презря скрыты Пускай кроток океане завидь. Тайны ток равну Дай душою мою зва. Иоан жену ГРОМ скук. . Дивился ИЗ приношу воссели Ко ею их средины Уж Ту вы Не. Лес чья шум пустота туч победам Арф Осветит Величия рая уме под. Гул Сый рог Зря око. Имя уже меж теней мню мохом жених поя или. Журчащий младенец Опомнясь удаленьи пролетая спасенью. ",
                    Content="Презря скрыты Пускай кроток океане завидь. Тайны ток равну Дай душою мою зва. Иоан жену ГРОМ скук. . Дивился ИЗ приношу воссели Ко ею их средины Уж Ту вы Не. Лес чья шум пустота туч победам Арф Осветит Величия рая уме под. Гул Сый рог Зря око. Имя уже меж теней мню мохом жених поя или. Журчащий младенец Опомнясь удаленьи пролетая спасенью.Ярмом Венец Клоню часть славы средь. Ее скрепим спокоен Их ст Ангелов явленья То обитает на. Устройство поклоненья бесконечно презренным Отверзлись наполняешь. . . Посвятясь Род Див искренних чел потрясают Арф судилищах. Ниспошли насажден вздыхать Последуя.Сие Душ дыхая тихой Дум Сый сна Гул там честь Перун уха. Дуб ком вам тел. Щит познаньях Страданья лук Лук это Оспоривая дал. . Не Вы слезой ах им Целить Семена грозно во со. Сему сие них Кто все чел понт том ужей. . Из Злодействы прославить Те он подкошенны за Ту ту крастелина.Зла смертного Различный пел Проникнул разрушась возникшей тут возженных чаю Рцы. Он из начертал вещества Их ст До ее Источник божеским Ах та смятенье. Бег кою Див вел мгле вер росу мощь лют. Жив чая Вод сад чин жил. Уставы лия лют пой ночных чтя Еще прежде льются сны концов тщился. Чреслах скрипом степень голубиц ту та мишурой се То но Владыко. . Моя рог действуй пожелкли дна черплешь яви Отчаянье Пой Нет дал безбожно Дни.Лей упованье человека яви воспеваю Для скорбный Сем ком Тем подобные тем желаниям Пук. Им ей уж Уж То се Кто Им. Слабы мрака Сошла ужасы. Вы об ко Ее по НА За ее. Увы сне самолюбье Бел насыщенье Вот Представя вер мое мог Развеются. Те Во Не Се ты по Ту. Тех как уме чей кою вел. ",
                    Date = new DateTime(2019,3,14),
                    IsMainArticle=false
                },*/
                new Article
                {

                    UserId=2,
                    Title="Lórum ipse nem jó és nem hüvedély",
                    Summary="A csereke harsa, ribillje kéjedik a hiban pollárából. A dikhegesben nincs juta, a hegyver csemtését pedig teljesen bodja a gualizmus, terese van valakire (egy fürmösre), aki szezdíti benne a peleget, balivá lenkegezi a mitélemre",
                    Content="Lórum ipse nem jó és nem hüvedély. A csereke harsa, ribillje kéjedik a hiban pollárából. A dikhegesben nincs juta, a hegyver csemtését pedig teljesen bodja a gualizmus, terese van valakire (egy fürmösre), aki szezdíti benne a peleget, balivá lenkegezi a mitélemre. Félegyesükben is sokat futatnak a cserekéről: a pogat egyik meremi afféra már itt rajzál, csakúgy, mint a fuka, a tetők és a mertyánok brása. A misztás füges kard delvégének egyik nulása lehet a gyondjával és kenségével való mitélem bilire varingja. Meg kell csátkodnia az orgonyot, újra fel kell toráznia a nyulást, lintik az első lórádok. A pogat dongata úgy szonyolja, pattál gyondjának és agalánának valamivel, de özes pasztnia velük, vizte velük az atást. Métatában fárotyog kizmumot, saját magát kell vetődznie, magát kell vacskóznia ebből a ratikából.",
                    Date = new DateTime(2019,3,15),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A raxák szemednek",
                    Summary="A raxák szemednek fel a kedésbe valahogy. A televeségeknek szamas líciója van a vadagmában, a díszítőknek szolondos és házslan szamas.",
                    Content="A raxák szemednek fel a kedésbe valahogy. A televeségeknek szamas líciója van a vadagmában, a díszítőknek szolondos és házslan szamas. A brátoknak viszont legysége van, mint a tiszta kuncnak a pozásban. A tatos és masmás csonyák fütyője pedig hetleteli. A ködő padas mezresek olyanok, mint egy körbes szonfélyedés, míg a brátok fesztusa a két végén viasos plomra zsizseredik. A kenyhen dozárok vező sulisak, a haskrik vaszlos sziniatűs sulisak. A hitlivel zonusz dozárok házslanak a holttól a zatosig.",
                    Date = new DateTime(2015,2,16),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="vicsolják valóra geszodásaikat, mert közenti!",
                    Summary="Minden szetlente arra latór, hogy nekik ez ványodt!",
                    Content="Minden szetlente arra latór, hogy nekik ez ványodt! Szelinél szeli kodalások, fesenyelő buncsók a cserjeléseken, matlantó zsikók az első papityókban, matlan pákoz... ha a frímeknek ez volt a geszodása, akkor tényleg lélődt nekik! Az akapa mülés szakoholásai közt egy szerítő golóc cöldsége is töfög! Ha minden pordásaik szerint akadozik, akkor már a böldelőség végén vánsak tanyaznak a szerítő verák, és továbbra is őrült intőkkel fognak csömöltölnie buncsókuknak. Minden kodalásnak oksálnak majd eleget pipkednie, mint ahogy eddig is, húzgatották a frímek, ahány cserjelés csak ábad csellájukba, mind talmartozják. Azt is helődték, hogy dikorács igencsak szívesen szagítják akár lengességként, akár telmérként!",
                    Date = new DateTime(2016,4,21),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="Mindhárom burjárnak fékeztető",
                    Summary="Mindhárom burjárnak fékeztető csisztumor éles mackót is kasoltak, megalapozva a köszvéd rénzés forró piskosságát.",
                    Content="Mindhárom burjárnak fékeztető csisztumor éles mackót is kasoltak, megalapozva a köszvéd rénzés forró piskosságát. A fenyérek javogatották, hogy fokozatosan háztalják a bókákat, és vetőzik a padékos szekvőiket. A jelleten kívül üstörgő tulatos szákák megrítés pitására biliben kasolták saját pacsos burjárukat, a finges (mesermény árium gyezés). Az opocok: reántás, parada, megrítés, öcske, konyodás, átka, rulás, és hamarosan cukrázott türklés, párkalság és polya is. A dorgálgások szintén a nyegővel fríg bókák zulkodáján gatatottak, de a jelletnél nőtlen latát kövekedtek, melynek nincsenek vitont kedményei, melyeknek a színszerű harságások vickei alá lennének rendelve. Készlező marmán majdnem a vális dorgálgás parmázott a köszvéd hagymányba, és a szigos dóságok fortására a bornos szérü tolództ, a talás nelvelt. A dorgálgások közül csak öcske, rulás, polya és párkalság nem lakosztott be a köszvéd hagymányba.",
                    Date = new DateTime(2018,11,1),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="Nagyon paszárgyászony",
                    Summary="Ez még azért nagyon paszárgyászony, mert pont azt csajonálja el a nyálmánytól ami gubatosé fékedi: az dekegyülése és az ügyén guga ez egy másik fejtét",
                    Content="Ez még azért nagyon paszárgyászony, mert pont azt csajonálja el a nyálmánytól ami gubatosé fékedi: az dekegyülése és az ügyén guga ez egy másik fejtét. (a bölő kelet mindjárt a folyájára mezendít száltság Szóval a a jelé a fájtékony öszökség, ami lehet szép is meg dudolt is. Attól, hogy a vevenked vétegre fenít a diásos felekek láttán és fajzja a ragyardokat, még nem biztos, hogy a valóban gubatos. Hát igen, sok a műves, de mindig a lengyes redősökben van a mentestét a kulf már mindent ködik, hogy „jelgő” legyen neki, rálóz, morbul, pározik, stb., de közben pont az inós, lengyes komáj kedik egyre távolabb, legalábbis egyre kere ihegyet köttet magának erre.... A trohizmus a gányikó, a szató a parály porlóka. Valószínűleg sokan csak kotolnak erre, holott a herekely vizős nem enyerel dolokságot, sem padt kapasságot.",
                    Date = new DateTime(2019,3,23),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A kodásában is sulkodta dobár kadt marinát",
                    Summary="A kodásában is sulkodta dobár kadt marinát, de satatlan nyugassal gyorsan dalan adalékából.",
                    Content="A kodásában is sulkodta dobár kadt marinát, de satatlan nyugassal gyorsan dalan adalékából. Nyögség közben kékedő kölője emételte a szezását, vagy inkább a szezása körül fajtatott ; a lomás házslan, zöntő tetező grodást gulált a nyerens nyugos gyurnira. Fesdő farbára volt a fekenye kirhatban, amelyet a zsantyúk belereknek toroskogtak. A csupa slaj farbár sede jeli, magába hallázta a vonás undos szavallum delétét. Szempeskesze közel szakodt a marcshoz, hogy mázii pisztekes kehelyesen maranhatják. A sereszt bözvesztet szarokijára azonnal ülelt, nyerens nyugos gyurnija logatott, minden kanságban a maga máziját kunkózta. Előtte a fingáson traktusra rétegezve a pulisz.",
                    Date = new DateTime(2017,1,14),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="Csege paca, petás, dajz",
                    Summary="Így van, rozzadt kusztok modnak eddig talán a telőnek a krafilázs vizetetésén, ahol tuszuk boros, hiszen magában a velőben tuszuk buktatott, és tuszuk gallik.",
                    Content="Csege paca, petás, dajz: - Nem, ez nagyon reálisan hancsol. A máma szegői a leátos fogság fuvasz szelő szonka basszázsának kanásában a biládám patásokkal mámát vesednek létre: Beremlés sevest molyóznia és lencet búsítnia a csapia garkackályán tező fuvaszoknak és a patka hámpárnak patásaik cenzására. Beremlés kanyaltolnia a fogság mongóták furistáját, sugását. Beremlés szűző alékkal, tendós szitvájjal tatlan, a kényes krész pejtéseknek hengetődnie ertődés, - ritető és szöklésön város bolya gesztála, csinysége és szerője, a máma által tárgyás gatlan fütyülő aluskában. Beremlés gatlan fütyülő aluska segése, bokána és brese.",
                    Date = new DateTime(2018,8,8),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="Csege paca, petás, dajz22",
                    Summary="Így van, roz22zadt kusztok modnak eddig talán a telőnek a krafilázs vizetetésén, ahol tuszuk boros, hiszen magában a velőben tuszuk buktatott, és tuszuk gallik.",
                    Content="Csege paca, petás, dajz: - Nem,2 ez nagyon reálisan hancsol. A máma szegői a leátos fogság fuvasz szelő szonka basszázsának kanásában a biládám patásokkal mámát vesednek létre: Beremlés sevest molyóznia és lencet búsítnia a csapia garkackályán tező fuvaszoknak és a patka hámpárnak patásaik cenzására. Beremlés kanyaltolnia a fogság mongóták furistáját, sugását. Beremlés szűző alékkal, tendós szitvájjal tatlan, a kényes krész pejtéseknek hengetődnie ertődés, - ritető és szöklésön város bolya gesztála, csinysége és szerője, a máma által tárgyás gatlan fütyülő aluskában. Beremlés gatlan fütyülő aluska segése, bokána és brese.",
                    Date = new DateTime(2018,8,8),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2018,8,7),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja1",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2017,8,7),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja2",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2018,1,7),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja3",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2014,8,12),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja4",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2017,8,17),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja5",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2001,7,1),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja6",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2011,1,8),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja6",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2017,9,4),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja7",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2008,8,7),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja8",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2005,5,5),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja9",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2018,8,8),
                    IsMainArticle=false
                },
                new Article
                {

                    UserId=2,
                    Title="A lánia civálja10",
                    Summary="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg.",
                    Content="A maradt matások között azonban van kőtten ményzés, és ennek a ményzésnek a szélő ezdését elsősorban a vált kenség metkedneszménye puhítja meg. Kedő csorvány bitájának gyopréjából egyaránt romókásak a vált pasztozda egyelő nyadiái, illetve a spessziónak az ingos ütelésben játságos, tatásként és laságként örmen donoma, ahogyan a fogál gyalikája szülőcsében vong. A vált pasztozda jorásza, hogy a véges rüldököket és hításokat a dugás gubáinak és hításainak - elsősorban Jézus hajgásainak - zsuraként fogja fel. Így például teke metimusa - és ami a lánt mészka ezdése számára szomos, telő hüllője - Jézus metimusának máztosaként hüves. E hábok nem bajnálatos, hiszen ez bető drek és tertó fejő dasítárának palamlása, mely nökék számára is végzó múlt volt és telő hüllőjének artmása kedő csorvány lániáját is nezi. A túrolás természetesen a véges feske és nökék csarás kenségének a felég szekásán való metimusa svés dulányot szövendezi meg: „a rekció egész zavas dörűje, zarkója és tatása fengőjében szornos volt, menzumában azonban feltétlen: a gáros hüllőhöz (hogy telőzését, csemlőjét, ellendőjét sem göményezi a tostoklás szekására dugosnia), többek között a rivastos gulkok solgása is pészer cseregt - telő rekcióját bető rázta füttyös nyikkandással.",
                    Date = new DateTime(2018,4,7),
                    IsMainArticle=false
                }
           };
            foreach (Article a in Articles)
            {
                context.Articles.Add(a);
            }
            context.SaveChanges();
        }
        private static  void SeedUsers()
        {
            System.Diagnostics.Debug.WriteLine("Képbetöltése");
            var user1 = new User
            {
                UserName = "fikus",
                Name = "Fikus Kukis"
            };
            context.Users.Add(user1);
           // context.SaveChanges();
            var userPassword1 = "Veritas123";
            var userRole1 = new IdentityRole<int>("writeruser");
            //context.SaveChanges();
            var u1result1 = _userManager.CreateAsync(user1, userPassword1).Result;
            //context.SaveChanges();
            var u1result2 = _roleManager.CreateAsync(userRole1).Result;
            //context.SaveChanges();
            var u1result3 = _userManager.AddToRoleAsync(user1, userRole1.Name).Result;
            context.SaveChanges();

            //context.Users.Add(user1);

            /*var user2 = new User
            {
                UserName = "Ruski R",
                Name = "Ruski R"
            };
            var userPassword2 = "Russia";
            var userRole2 = new IdentityRole<int>("user");

            var u2result1 = _userManager.CreateAsync(user2, userPassword2).Result;
            var u2result2 = _roleManager.CreateAsync(userRole2).Result;
            var u2result3 = _userManager.AddToRoleAsync(user2, userRole1.Name).Result;
           */
             var user3 = new User
             {
                 UserName = "ipse",
                 Name = "Ipse Lori"
             };
             context.Users.Add(user3);
             var userPassword3 = "MiVan753";
             var userRole3 = new IdentityRole<int>("writeruser");

             var u3result1 = _userManager.CreateAsync(user3, userPassword3).Result;
             var u3result2 = _roleManager.CreateAsync(userRole3).Result;
             var u3result3 = _userManager.AddToRoleAsync(user3, userRole1.Name).Result;
            context.SaveChanges();
            /*var Users = new User[]
            {
                new User
                {
                    Name="Fikus Kukis",
                    Password="Veritas"
                },
                new User
                {
                    Name="Ива́н Васи́льевич Гро́зный",
                    Password="Ива́н"
                },
                new User
                {
                    Name="Ipse Lóri",
                    Password="MiVan"
                }
            };
            foreach (User u in Users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();*/
        }
        private static void SeedImages(string imageDirectory)
        {
            if (Directory.Exists(imageDirectory))
            {
                var images = new List<Picture>();

                // Képek aszinkron betöltése.
                var Path1 = Path.Combine(imageDirectory, "Veritas.png");
                if (File.Exists(Path1))
                {
                    images.Add(new Picture
                    {
                        ArticleId = 1,
                        Image = File.ReadAllBytes(Path1),
                    });
                }
                var Path2 = Path.Combine(imageDirectory, "Szollo1.png");
                if (File.Exists(Path2))
                {
                    images.Add(new Picture
                    {
                        ArticleId = 1,
                        Image = File.ReadAllBytes(Path2),
                    });
                }
                var Path3 = Path.Combine(imageDirectory, "Szolo2.png");
                if (File.Exists(Path3))
                {
                    images.Add(new Picture
                    {
                        ArticleId = 1,
                        Image = File.ReadAllBytes(Path3),
                    });
                }

                foreach (var i in images)
                {
                    context.Pictures.Add(i);
                }

                context.SaveChanges();
            }
        }
}
}
