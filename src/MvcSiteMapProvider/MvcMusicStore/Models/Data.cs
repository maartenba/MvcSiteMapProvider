using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// ----------------------------------------------------------------------------------
// This file exists solely to keep this project thinking it's using Entity Framework.
// It does not hold *any* best practices!
// ----------------------------------------------------------------------------------

namespace MvcMusicStore.Models
{
    public class Data
    {
        public static List<Genre> Genres { get; set; }
        public static List<Artist> Artists { get; set; }
        public static List<Album> Albums { get; set; }
        public static List<Cart> Carts { get; set; }
        public static List<Order> Orders { get; set; }
        public static List<OrderDetail> OrderDetails { get; set; } 

        static Data()
        {
            Genres = new List<Genre>
                {
                    new Genre(1, "Rock", "Rock and Roll is a form of rock music developed in the 1950s and 1960s. Rock music combines many kinds of music from the United States, such as country music, folk music, church music, work songs, blues and jazz."),
                    new Genre(2, "Jazz", "Jazz is a type of music which was invented in the United States. Jazz music combines African-American music with European music. Some common jazz instruments include the saxophone, trumpet, piano, double bass, and drums."),
                    new Genre(3, "Metal", "Heavy Metal is a loud, aggressive style of Rock music. The bands who play heavy-metal music usually have one or two guitars, a bass guitar and drums. In some bands, electronic keyboards, organs, or other instruments are used. Heavy metal songs are loud and powerful-sounding, and have strong rhythms that are repeated. There are many different types of Heavy Metal, some of which are described below. Heavy metal bands sometimes dress in jeans, leather jackets, and leather boots, and have long hair. Heavy metal bands sometimes behave in a dramatic way when they play their instruments or sing. However, many heavy metal bands do not like to do this."),
                    new Genre(4, "Alternative", "Alternative rock is a type of rock music that became popular in the 1980s and became widely popular in the 1990s. Alternative rock is made up of various subgenres that have come out of the indie music scene since the 1980s, such as grunge, indie rock, Britpop, gothic rock, and indie pop. These genres are sorted by their collective types of punk, which laid the groundwork for alternative music in the 1970s."),
                    new Genre(5, "Disco", "Disco is a style of pop music that was popular in the mid-1970s. Disco music has a strong beat that people can dance to. People usually dance to disco music at bars called disco clubs. The word \"disco\" is also used to refer to the style of dancing that people do to disco music, or to the style of clothes that people wear to go disco dancing. Disco was at its most popular in the United States and Europe in the 1970s and early 1980s. Disco was brought into the mainstream by the hit movie Saturday Night Fever, which was released in 1977. This movie, which starred John Travolta, showed people doing disco dancing. Many radio stations played disco in the late 1970s."),
                    new Genre(6, "Blues", "The blues is a form of music that started in the United States during the start of the 20th century. It was started by former African slaves from spirituals, praise songs, and chants. The first blues songs were called Delta blues. These songs came from the area near the mouth of the Mississippi River."),
                    new Genre(7, "Latin", "Latin American music is the music of all countries in Latin America (and the Caribbean) and comes in many varieties. Latin America is home to musical styles such as the simple, rural conjunto music of northern Mexico, the sophisticated habanera of Cuba, the rhythmic sounds of the Puerto Rican plena, the symphonies of Heitor Villa-Lobos, and the simple and moving Andean flute. Music has played an important part recently in Latin America's politics, the nueva canción movement being a prime example. Latin music is very diverse, with the only truly unifying thread being the use of Latin-derived languages, predominantly the Spanish language, the Portuguese language in Brazil, and to a lesser extent, Latin-derived creole languages, such as those found in Haiti."),
                    new Genre(8, "Reggae", "Reggae is a music genre first developed in Jamaica in the late 1960s. While sometimes used in a broader sense to refer to most types of Jamaican music, the term reggae more properly denotes a particular music style that originated following on the development of ska and rocksteady."),
                    new Genre(9, "Pop", "Pop music is a music genre that developed from the mid-1950s as a softer alternative to rock 'n' roll and later to rock music. It has a focus on commercial recording, often oriented towards a youth market, usually through the medium of relatively short and simple love songs. While these basic elements of the genre have remained fairly constant, pop music has absorbed influences from most other forms of popular music, particularly borrowing from the development of rock music, and utilizing key technological innovations to produce new variations on existing themes."),
                    new Genre(10, "Classical", "Classical music is a very general term which normally refers to the standard music of countries in the Western world. It is music that has been composed by musicians who are trained in the art of writing music (composing) and written down in music notation so that other musicians can play it. Classical music can also be described as \"art music\" because great art (skill) is needed to compose it and to perform it well. Classical music differs from pop music because it is not made just in order to be popular for a short time or just to be a commercial success.")
                };

            Artists = new List<Artist>
                {
                    new Artist(1, "AC/DC"),
                    new Artist(2, "Accept"),
                    new Artist(3, "Aerosmith"),
                    new Artist(4, "Alanis Morissette"),
                    new Artist(5, "Alice In Chains"),
                    new Artist(6, "Antônio Carlos Jobim"),
                    new Artist(7, "Apocalyptica"),
                    new Artist(8, "Audioslave"),
                    new Artist(10, "Billy Cobham"),
                    new Artist(11, "Black Label Society"),
                    new Artist(12, "Black Sabbath"),
                    new Artist(14, "Bruce Dickinson"),
                    new Artist(15, "Buddy Guy"),
                    new Artist(16, "Caetano Veloso"),
                    new Artist(17, "Chico Buarque"),
                    new Artist(18, "Chico Science & Nação Zumbi"),
                    new Artist(19, "Cidade Negra"),
                    new Artist(20, "Cláudio Zoli"),
                    new Artist(21, "Various Artists"),
                    new Artist(22, "Led Zeppelin"),
                    new Artist(23, "Frank Zappa & Captain Beefheart"),
                    new Artist(24, "Marcos Valle"),
                    new Artist(27, "Gilberto Gil"),
                    new Artist(37, "Ed Motta"),
                    new Artist(41, "Elis Regina"),
                    new Artist(42, "Milton Nascimento"),
                    new Artist(46, "Jorge Ben"),
                    new Artist(50, "Metallica"),
                    new Artist(51, "Queen"),
                    new Artist(52, "Kiss"),
                    new Artist(53, "Spyro Gyra"),
                    new Artist(55, "David Coverdale"),
                    new Artist(56, "Gonzaguinha"),
                    new Artist(58, "Deep Purple"),
                    new Artist(59, "Santana"),
                    new Artist(68, "Miles Davis"),
                    new Artist(72, "Vinícius De Moraes"),
                    new Artist(76, "Creedence Clearwater Revival"),
                    new Artist(77, "Cássia Eller"),
                    new Artist(79, "Dennis Chambers"),
                    new Artist(80, "Djavan"),
                    new Artist(81, "Eric Clapton"),
                    new Artist(82, "Faith No More"),
                    new Artist(83, "Falamansa"),
                    new Artist(84, "Foo Fighters"),
                    new Artist(86, "Funk Como Le Gusta"),
                    new Artist(87, "Godsmack"),
                    new Artist(88, "Guns N' Roses"),
                    new Artist(89, "Incognito"),
                    new Artist(90, "Iron Maiden"),
                    new Artist(92, "Jamiroquai"),
                    new Artist(94, "Jimi Hendrix"),
                    new Artist(95, "Joe Satriani"),
                    new Artist(96, "Jota Quest"),
                    new Artist(98, "Judas Priest"),
                    new Artist(99, "Legião Urbana"),
                    new Artist(100, "Lenny Kravitz"),
                    new Artist(101, "Lulu Santos"),
                    new Artist(102, "Marillion"),
                    new Artist(103, "Marisa Monte"),
                    new Artist(105, "Men At Work"),
                    new Artist(106, "Motörhead"),
                    new Artist(109, "Mötley Crüe"),
                    new Artist(110, "Nirvana"),
                    new Artist(111, "O Terço"),
                    new Artist(112, "Olodum"),
                    new Artist(113, "Os Paralamas Do Sucesso"),
                    new Artist(114, "Ozzy Osbourne"),
                    new Artist(115, "Page & Plant"),
                    new Artist(117, "Paul D'Ianno"),
                    new Artist(118, "Pearl Jam"),
                    new Artist(120, "Pink Floyd"),
                    new Artist(124, "R.E.M."),
                    new Artist(126, "Raul Seixas"),
                    new Artist(127, "Red Hot Chili Peppers"),
                    new Artist(128, "Rush"),
                    new Artist(130, "Skank"),
                    new Artist(132, "Soundgarden"),
                    new Artist(133, "Stevie Ray Vaughan & Double Trouble"),
                    new Artist(134, "Stone Temple Pilots"),
                    new Artist(135, "System Of A Down"),
                    new Artist(136, "Terry Bozzio, Tony Levin & Steve Stevens"),
                    new Artist(137, "The Black Crowes"),
                    new Artist(139, "The Cult"),
                    new Artist(140, "The Doors"),
                    new Artist(141, "The Police"),
                    new Artist(142, "The Rolling Stones"),
                    new Artist(144, "The Who"),
                    new Artist(145, "Tim Maia"),
                    new Artist(150, "U2"),
                    new Artist(151, "UB40"),
                    new Artist(152, "Van Halen"),
                    new Artist(153, "Velvet Revolver"),
                    new Artist(155, "Zeca Pagodinho"),
                    new Artist(157, "Dread Zeppelin"),
                    new Artist(179, "Scorpions"),
                    new Artist(196, "Cake"),
                    new Artist(197, "Aisha Duo"),
                    new Artist(200, "The Posies"),
                    new Artist(201, "Luciana Souza/Romero Lubambo"),
                    new Artist(202, "Aaron Goldberg"),
                    new Artist(203, "Nicolaus Esterhazy Sinfonia"),
                    new Artist(204, "Temple of the Dog"),
                    new Artist(205, "Chris Cornell"),
                    new Artist(206, "Alberto Turco & Nova Schola Gregoriana"),
                    new Artist(208, "English Concert & Trevor Pinnock"),
                    new Artist(211, "Wilhelm Kempff"),
                    new Artist(212, "Yo-Yo Ma"),
                    new Artist(213, "Scholars Baroque Ensemble"),
                    new Artist(217, "Royal Philharmonic Orchestra & Sir Thomas Beecham"),
                    new Artist(219, "Britten Sinfonia, Ivor Bolton & Lesley Garrett"),
                    new Artist(221, "Sir Georg Solti & Wiener Philharmoniker"),
                    new Artist(223, "London Symphony Orchestra & Sir Charles Mackerras"),
                    new Artist(224, "Barry Wordsworth & BBC Concert Orchestra"),
                    new Artist(226, "Eugene Ormandy"),
                    new Artist(229, "Boston Symphony Orchestra & Seiji Ozawa"),
                    new Artist(230, "Aaron Copland & London Symphony Orchestra"),
                    new Artist(231, "Ton Koopman"),
                    new Artist(232, "Sergei Prokofiev & Yuri Temirkanov"),
                    new Artist(233, "Chicago Symphony Orchestra & Fritz Reiner"),
                    new Artist(234, "Orchestra of The Age of Enlightenment"),
                    new Artist(236, "James Levine"),
                    new Artist(237, "Berliner Philharmoniker & Hans Rosbaud"),
                    new Artist(238, "Maurizio Pollini"),
                    new Artist(240, "Gustav Mahler"),
                    new Artist(242, "Edo de Waart & San Francisco Symphony"),
                    new Artist(244, "Choir Of Westminster Abbey & Simon Preston"),
                    new Artist(245, "Michael Tilson Thomas & San Francisco Symphony"),
                    new Artist(247, "The King's Singers"),
                    new Artist(248, "Berliner Philharmoniker & Herbert Von Karajan"),
                    new Artist(250, "Christopher O'Riley"),
                    new Artist(251, "Fretwork"),
                    new Artist(252, "Amy Winehouse"),
                    new Artist(253, "Calexico"),
                    new Artist(255, "Yehudi Menuhin"),
                    new Artist(258, "Les Arts Florissants & William Christie"),
                    new Artist(259, "The 12 Cellists of The Berlin Philharmonic"),
                    new Artist(260, "Adrian Leaper & Doreen de Feis"),
                    new Artist(261, "Roger Norrington, London Classical Players"),
                    new Artist(264, "Kent Nagano and Orchestre de l'Opéra de Lyon"),
                    new Artist(265, "Julian Bream"),
                    new Artist(266, "Martin Roscoe"),
                    new Artist(267, "Göteborgs Symfoniker & Neeme Järvi"),
                    new Artist(270, "Gerald Moore"),
                    new Artist(271, "Mela Tenenbaum, Pro Musica Prague & Richard Kapp"),
                    new Artist(274, "Nash Ensemble"),
                    new Artist(276, "Chic"),
                    new Artist(277, "Anita Ward"),
                    new Artist(278, "Donna Summer")
                };

            Albums = new List<Album>
                {
                    new Album(386, 1, 1, "For Those About To Rock We Salute You", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(387, 1, 1, "Let There Be Rock", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(388, 1, 100, "Greatest Hits", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(389, 1, 102, "Misplaced Childhood", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(390, 1, 105, "The Best Of Men At Work", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(392, 1, 110, "Nevermind", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(393, 1, 111, "Compositores", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(394, 1, 114, "Bark at the Moon (Remastered)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(395, 1, 114, "Blizzard of Ozz", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(396, 1, 114, "Diary of a Madman (Remastered)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(397, 1, 114, "No More Tears (Remastered)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(398, 1, 114, "Speak of the Devil", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(399, 1, 115, "Walking Into Clarksdale", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(400, 1, 117, "The Beast Live", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(401, 1, 118, "Live On Two Legs [Live]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(402, 1, 118, "Riot Act", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(403, 1, 118, "Ten", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(404, 1, 118, "Vs.", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(405, 1, 120, "Dark Side Of The Moon", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(406, 1, 124, "New Adventures In Hi-Fi", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(407, 1, 126, "Raul Seixas", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(408, 1, 127, "By The Way", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(409, 1, 127, "Californication", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(410, 1, 128, "Retrospective I (1974-1980)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(411, 1, 130, "Maquinarama", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(412, 1, 130, "O Samba Poconé", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(413, 1, 132, "A-Sides", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(414, 1, 134, "Core", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(415, 1, 136, "[1997] Black Light Syndrome", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(416, 1, 139, "Beyond Good And Evil", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(418, 1, 140, "The Doors", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(419, 1, 141, "The Police Greatest Hits", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(420, 1, 142, "Hot Rocks, 1964-1971 (Disc 1)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(421, 1, 142, "No Security", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(422, 1, 142, "Voodoo Lounge", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(423, 1, 144, "My Generation - The Very Best Of The Who", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(424, 1, 150, "Achtung Baby", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(425, 1, 150, "B-Sides 1980-1990", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(426, 1, 150, "How To Dismantle An Atomic Bomb", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(427, 1, 150, "Pop", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(428, 1, 150, "Rattle And Hum", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(429, 1, 150, "The Best Of 1980-1990", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(430, 1, 150, "War", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(431, 1, 150, "Zooropa", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(432, 1, 152, "Diver Down", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(433, 1, 152, "The Best Of Van Halen, Vol. I", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(434, 1, 152, "Van Halen III", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(435, 1, 152, "Van Halen", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(436, 1, 153, "Contraband", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(437, 1, 157, "Un-Led-Ed", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(439, 1, 2, "Balls to the Wall", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(440, 1, 2, "Restless and Wild", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(441, 1, 200, "Every Kind of Light", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(442, 1, 22, "BBC Sessions [Disc 1] [Live]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(443, 1, 22, "BBC Sessions [Disc 2] [Live]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(444, 1, 22, "Coda", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(445, 1, 22, "Houses Of The Holy", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(446, 1, 22, "In Through The Out Door", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(447, 1, 22, "IV", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(448, 1, 22, "Led Zeppelin I", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(449, 1, 22, "Led Zeppelin II", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(450, 1, 22, "Led Zeppelin III", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(451, 1, 22, "Physical Graffiti [Disc 1]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(452, 1, 22, "Physical Graffiti [Disc 2]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(453, 1, 22, "Presence", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(454, 1, 22, "The Song Remains The Same (Disc 1)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(455, 1, 22, "The Song Remains The Same (Disc 2)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(456, 1, 23, "Bongo Fury", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(457, 1, 3, "Big Ones", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(458, 1, 4, "Jagged Little Pill", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(459, 1, 5, "Facelift", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(460, 1, 51, "Greatest Hits I", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(461, 1, 51, "Greatest Hits II", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(462, 1, 51, "News Of The World", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(463, 1, 52, "Greatest Kiss", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(464, 1, 52, "Unplugged [Live]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(465, 1, 55, "Into The Light", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(466, 1, 58, "Come Taste The Band", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(467, 1, 58, "Deep Purple In Rock", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(468, 1, 58, "Fireball", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(469, 1, 58, "Machine Head", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(470, 1, 58, "MK III The Final Concerts [Disc 1]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(471, 1, 58, "Purpendicular", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(472, 1, 58, "Slaves And Masters", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(473, 1, 58, "Stormbringer", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(474, 1, 58, "The Battle Rages On", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(475, 1, 58, "The Final Concerts (Disc 2)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(476, 1, 59, "Santana - As Years Go By", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(477, 1, 59, "Santana Live", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(478, 1, 59, "Supernatural", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(479, 1, 76, "Chronicle, Vol. 1", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(480, 1, 76, "Chronicle, Vol. 2", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(481, 1, 8, "Audioslave", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(482, 1, 82, "King For A Day Fool For A Lifetime", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(483, 1, 84, "In Your Honor [Disc 1]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(484, 1, 84, "In Your Honor [Disc 2]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(485, 1, 84, "The Colour And The Shape", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(486, 1, 88, "Appetite for Destruction", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(487, 1, 88, "Use Your Illusion I", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(488, 1, 90, "A Matter of Life and Death", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(489, 1, 90, "Brave New World", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(490, 1, 90, "Fear Of The Dark", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(491, 1, 90, "Live At Donington 1992 (Disc 1)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(492, 1, 90, "Live At Donington 1992 (Disc 2)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(493, 1, 90, "Rock In Rio [CD2]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(494, 1, 90, "The Number of The Beast", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(495, 1, 90, "The X Factor", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(496, 1, 90, "Virtual XI", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(497, 1, 92, "Emergency On Planet Earth", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(498, 1, 94, "Are You Experienced?", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(499, 1, 95, "Surfing with the Alien (Remastered)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(500, 10, 203, "The Best of Beethoven", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(504, 10, 208, "Pachelbel: Canon & Gigue", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(507, 10, 211, "Bach: Goldberg Variations", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(508, 10, 212, "Bach: The Cello Suites", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(509, 10, 213, "Handel: The Messiah (Highlights)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(513, 10, 217, "Haydn: Symphonies 99 - 104", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(515, 10, 219, "A Soprano Inspired", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(517, 10, 221, "Wagner: Favourite Overtures", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(519, 10, 223, "Tchaikovsky: The Nutcracker", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(520, 10, 224, "The Last Night of the Proms", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(523, 10, 226, "Respighi:Pines of Rome", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(524, 10, 226, "Strauss: Waltzes", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(525, 10, 229, "Carmina Burana", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(526, 10, 230, "A Copland Celebration, Vol. I", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(527, 10, 231, "Bach: Toccata & Fugue in D Minor", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(528, 10, 232, "Prokofiev: Symphony No.1", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(529, 10, 233, "Scheherazade", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(530, 10, 234, "Bach: The Brandenburg Concertos", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(532, 10, 236, "Mascagni: Cavalleria Rusticana", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(533, 10, 237, "Sibelius: Finlandia", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(537, 10, 242, "Adams, John: The Chairman Dances", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(539, 10, 245, "Berlioz: Symphonie Fantastique", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(540, 10, 245, "Prokofiev: Romeo & Juliet", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(542, 10, 247, "English Renaissance", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(544, 10, 248, "Mozart: Symphonies Nos. 40 & 41", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(546, 10, 250, "SCRIABIN: Vers la flamme", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(548, 10, 255, "Bartok: Violin & Viola Concertos", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(551, 10, 259, "South American Getaway", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(552, 10, 260, "Górecki: Symphony No. 3", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(553, 10, 261, "Purcell: The Fairy Queen", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(556, 10, 264, "Weill: The Seven Deadly Sins", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(558, 10, 266, "Szymanowski: Piano Works, Vol. 1", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(559, 10, 267, "Nielsen: The Six Symphonies", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(562, 10, 274, "Mozart: Chamber Music", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(563, 2, 10, "The Best Of Billy Cobham", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(564, 2, 197, "Quiet Songs", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(565, 2, 202, "Worlds", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(566, 2, 27, "Quanta Gente Veio ver--Bônus De Carnaval", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(567, 2, 53, "Heart of the Night", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(568, 2, 53, "Morning Dance", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(569, 2, 6, "Warner 25 Anos", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(570, 2, 68, "Miles Ahead", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(571, 2, 68, "The Essential Miles Davis [Disc 1]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(572, 2, 68, "The Essential Miles Davis [Disc 2]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(573, 2, 79, "Outbreak", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(574, 2, 89, "Blue Moods", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(575, 3, 100, "Greatest Hits", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(576, 3, 106, "Ace Of Spades", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(577, 3, 109, "Motley Crue Greatest Hits", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(578, 3, 11, "Alcohol Fueled Brewtality Live! [Disc 1]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(579, 3, 11, "Alcohol Fueled Brewtality Live! [Disc 2]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(580, 3, 114, "Tribute", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(581, 3, 12, "Black Sabbath Vol. 4 (Remaster)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(582, 3, 12, "Black Sabbath", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(583, 3, 135, "Mezmerize", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(584, 3, 14, "Chemical Wedding", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(585, 3, 50, "...And Justice For All", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(586, 3, 50, "Black Album", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(587, 3, 50, "Garage Inc. (Disc 1)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(588, 3, 50, "Garage Inc. (Disc 2)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(589, 3, 50, "Load", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(590, 3, 50, "Master Of Puppets", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(591, 3, 50, "ReLoad", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(592, 3, 50, "Ride The Lightning", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(593, 3, 50, "St. Anger", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(594, 3, 7, "Plays Metallica By Four Cellos", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(595, 3, 87, "Faceless", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(596, 3, 88, "Use Your Illusion II", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(597, 3, 90, "A Real Dead One", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(598, 3, 90, "A Real Live One", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(599, 3, 90, "Live After Death", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(600, 3, 90, "No Prayer For The Dying", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(601, 3, 90, "Piece Of Mind", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(602, 3, 90, "Powerslave", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(603, 3, 90, "Rock In Rio [CD1]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(604, 3, 90, "Rock In Rio [CD2]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(605, 3, 90, "Seventh Son of a Seventh Son", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(606, 3, 90, "Somewhere in Time", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(607, 3, 90, "The Number of The Beast", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(608, 3, 98, "Living After Midnight", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(609, 4, 196, "Cake: B-Sides and Rarities", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(610, 4, 204, "Temple of the Dog", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(611, 4, 205, "Carry On", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(612, 4, 253, "Carried to Dust (Bonus Track Version)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(613, 4, 8, "Revelations", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(614, 6, 133, "In Step", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(615, 6, 137, "Live [Disc 1]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(616, 6, 137, "Live [Disc 2]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(618, 6, 81, "The Cream Of Clapton", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(619, 6, 81, "Unplugged", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(620, 6, 90, "Iron Maiden", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(623, 7, 103, "Barulhinho Bom", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(624, 7, 112, "Olodum", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(625, 7, 113, "Acústico MTV", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(626, 7, 113, "Arquivo II", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(627, 7, 113, "Arquivo Os Paralamas Do Sucesso", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(628, 7, 145, "Serie Sem Limite (Disc 1)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(629, 7, 145, "Serie Sem Limite (Disc 2)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(630, 7, 155, "Ao Vivo [IMPORT]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(631, 7, 16, "Prenda Minha", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(632, 7, 16, "Sozinho Remix Ao Vivo", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(633, 7, 17, "Minha Historia", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(634, 7, 18, "Afrociberdelia", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(635, 7, 18, "Da Lama Ao Caos", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(636, 7, 20, "Na Pista", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(637, 7, 201, "Duos II", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(638, 7, 21, "Sambas De Enredo 2001", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(639, 7, 21, "Vozes do MPB", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(640, 7, 24, "Chill: Brazil (Disc 1)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(641, 7, 27, "Quanta Gente Veio Ver (Live)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(642, 7, 37, "The Best of Ed Motta", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(643, 7, 41, "Elis Regina-Minha História", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(644, 7, 42, "Milton Nascimento Ao Vivo", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(645, 7, 42, "Minas", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(646, 7, 46, "Jorge Ben Jor 25 Anos", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(647, 7, 56, "Meus Momentos", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(648, 7, 6, "Chill: Brazil (Disc 2)", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(649, 7, 72, "Vinicius De Moraes", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(651, 7, 77, "Cássia Eller - Sem Limite [Disc 1]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(652, 7, 80, "Djavan Ao Vivo - Vol. 02", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(653, 7, 80, "Djavan Ao Vivo - Vol. 1", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(654, 7, 81, "Unplugged", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(655, 7, 83, "Deixa Entrar", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(656, 7, 86, "Roda De Funk", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(657, 7, 96, "Jota Quest-1995", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(659, 7, 99, "Mais Do Mesmo", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(660, 8, 100, "Greatest Hits", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(661, 8, 151, "UB40 The Best Of - Volume Two [UK]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(662, 8, 19, "Acústico MTV [Live]", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(663, 8, 19, "Cidade Negra - Hits", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(665, 9, 21, "Axé Bahia 2001", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(666, 9, 252, "Frank", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(667, 5, 276, "Le Freak", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(668, 5, 278, "MacArthur Park Suite", 8.99, "/Content/Images/placeholder.gif"),
                    new Album(669, 5, 277, "Ring My Bell", 8.99, "/Content/Images/placeholder.gif")
                };

            Carts = new List<Cart>();
            Orders = new List<Order>();
            OrderDetails = new List<OrderDetail>();
        }
    }

    public class MusicStoreEntities
        : IDisposable
    {
        public List<Genre> Genres
        {
            get { return Data.Genres; }
        }

        public List<Artist> Artists
        {
            get { return Data.Artists; }
        }

        public List<Album> Albums
        {
            get { return Data.Albums; }
        }

        public List<Cart> Carts
        {
            get { return Data.Carts; }
        }

        public List<Order> Orders
        {
            get { return Data.Orders; }
        }

        public List<OrderDetail> OrderDetails
        {
            get { return Data.OrderDetails; }
        }

        public void Dispose()
        {
        }

        public void SaveChanges()
        {
        }

        public void DeleteObject(object entity)
        {
            if (entity is Genre) Genres.Remove((Genre)entity);
            if (entity is Artist) Artists.Remove((Artist)entity);
            if (entity is Album) Albums.Remove((Album)entity);
            if (entity is Cart) Carts.Remove((Cart)entity);
            if (entity is Order) Orders.Remove((Order)entity);
            if (entity is OrderDetail) OrderDetails.Remove((OrderDetail)entity);
        }

        public void AddToGenres(Genre genre)
        {
            Genres.Add(genre);
        }

        public void AddToArtists(Artist artist)
        {
            Artists.Add(artist);
        }

        public void AddToAlbums(Album album)
        {
            Albums.Add(album);
        }

        public void AddToCarts(Cart cartItem)
        {
            Carts.Add(cartItem);
        }

        public void AddToOrders(Order order)
        {
            Orders.Add(order);
        }

        public void AddToOrderDetails(OrderDetail orderDetail)
        {
            OrderDetails.Add(orderDetail);
        }
    }

    public static class ListExtensions
    {
        public static List<T> Include<T>(this List<T> current, string dummy)
            where T : class
        {
            return current;
        }

        public static void AddObject<T>(this List<T> current, T entity)
            where T : class
        {
            current.Add(entity);
        }

        public static void DeleteObject<T>(this List<T> current, T entity)
            where T : class
        {
            current.Remove(entity);
        }
    }

    public partial class Cart
    {
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int AlbumId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }

        public Album Album
        {
            get { return Data.Albums.FirstOrDefault(a => a.AlbumId == AlbumId); }
        }
    }

    public partial class Order
    {
        public int OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public string Username { get; set; }
        public double Total { get; set; }

        public List<OrderDetail> OrderDetails
        {
            get { return Data.OrderDetails.Where(a => a.OrderId == OrderId).ToList(); }
        }
    }

    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int AlbumId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public Order Order
        {
            get { return Data.Orders.FirstOrDefault(a => a.OrderId == OrderId); }
        }

        public Album Album
        {
            get { return Data.Albums.FirstOrDefault(a => a.AlbumId == AlbumId); }
        }
    }

    public partial class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Genre()
        {
        }

        public Genre(int genreId, string name, string description)
        {
            GenreId = genreId;
            Name = name;
            Description = description;
        }

        public List<Album> Albums
        {
            get { return Data.Albums.Where(a => a.GenreId == GenreId).ToList(); }
        }
    }

    public partial class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }

        public Artist()
        {
        }

        public Artist(int artistId, string name)
        {
            ArtistId = artistId;
            Name = name;
        }

        public List<Album> Albums
        {
            get { return Data.Albums.Where(a => a.ArtistId == ArtistId).ToList(); }
        }
    }

    public partial class Album
    {
        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string AlbumArtUrl { get; set; }

        public Album()
        {
        }

        public Album(int albumId, int genreId, int artistId, string title, double price, string albumArtUrl)
        {
            AlbumId = albumId;
            GenreId = genreId;
            ArtistId = artistId;
            Title = title;
            Price = price;
            AlbumArtUrl = albumArtUrl;
        }

        public Artist Artist
        {
            get { return Data.Artists.FirstOrDefault(a => a.ArtistId == ArtistId); }
        }

        public Genre Genre
        {
            get { return Data.Genres.FirstOrDefault(a => a.GenreId == GenreId); }
        }

        public List<OrderDetail> OrderDetails
        {
            get { return Data.OrderDetails.Where(a => a.AlbumId == AlbumId).ToList(); }
        }
    }
}