Library library1 = new();
library1 += new Livre("Livre 1", 1, 1, "Auteur 1");
library1 += new CD("CD 1", 2, 1, "Artiste 1");
library1 += new DVD("DVD 1", 3, 5, "160min");

library1.AfficherLibrairieStat();

try {
    library1.EmprunterMedia(new Livre("Livre 1", 1, 1, "Auteur 1"));
} catch (Exception e) {
    Console.WriteLine(e.Message);
}

library1.AfficherLibrairieStat();

try {
    library1.RetournerMedia(new Livre("Livre 1", 1, 1, "Auteur 1"));
} catch (Exception e) {
    Console.WriteLine(e.Message);
}

library1.AfficherLibrairieStat();

library1.Medias.ForEach(x => x.AfficherInfos());

try {
    library1.SauvegarderBibliotheque("library1.json");
} catch (Exception e) {
    Console.WriteLine(e.Message);
}

try {
    library1.ChargerBibliotheque("library1.json");
} catch (Exception e) {
    Console.WriteLine(e.Message);
}

library1.AfficherLibrairieStat();