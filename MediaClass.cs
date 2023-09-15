using Newtonsoft.Json;

[Serializable]
public class Media {
    
    private string titre;

    public string Titre {
        get { return titre; }
        set { titre = value; }
    }
    private int numero_reference;

    public int Numero_reference {
        get { return numero_reference; }
        set { numero_reference = value; }
    }
    private int nb_exemplaires_dispo;

    public int Nb_exemplaires_dispo {
        get { return nb_exemplaires_dispo; }
        set { nb_exemplaires_dispo = value; }
    }

    public Media(string titre, int numero_reference, int nb_exemplaires_dispo){
        this.titre = titre;
        this.numero_reference = numero_reference;
        this.nb_exemplaires_dispo = nb_exemplaires_dispo;
    }

    public virtual void AfficherInfos(){
        Console.WriteLine("Titre : " + titre);
        Console.WriteLine("Numero de reference : " + numero_reference);
        Console.WriteLine("Nombre d'exemplaires disponibles : " + nb_exemplaires_dispo);
    }
}

public class Livre : Media {
    private string auteur { get; set; }

    public Livre(string titre, int numero_reference, int nb_exemplaires_dispo, string auteur) : base(titre, numero_reference, nb_exemplaires_dispo){
        this.auteur = auteur;
    }

    public override void AfficherInfos(){
        base.AfficherInfos();
        Console.WriteLine("Auteur : " + auteur);
    }
}

public class CD : Media {
    private string artiste { get; set; }

    public CD(string titre, int numero_reference, int nb_exemplaires_dispo, string artiste) : base(titre, numero_reference, nb_exemplaires_dispo){
        this.artiste = artiste;
    }

    public override void AfficherInfos(){
        base.AfficherInfos();
        Console.WriteLine("Artiste : " + artiste);
    }
}

public class DVD : Media {
    private string duree { get; set; }

    public DVD(string titre, int numero_reference, int nb_exemplaires_dispo, string duree) : base(titre, numero_reference, nb_exemplaires_dispo){
        this.duree = duree;
    }

    public override void AfficherInfos(){
        base.AfficherInfos();
        Console.WriteLine("Duree : " + duree);
    }
}

public class Emprunt{
    private List<Tuple<string, List<string>>> emprunts = new List<Tuple<string, List<string>>>();

    public int CountEmprunt(){
        return emprunts.Sum(x => x.Item2.Count());
    }

    public void AjouterEmprunt(string nom, string emprunts){
        var emprunt = this.emprunts.FirstOrDefault(x => x.Item1 == nom);
        if(emprunt != null){
            emprunt.Item2.Add(emprunts);
        }
        else {
            this.emprunts.Add(new Tuple<string, List<string>>(nom, new List<string>() { emprunts }));   
        }
    }

    public void RetournerEmprunt(string nom, string emprunts){
        var emprunt = this.emprunts.FirstOrDefault(x => x.Item1 == nom);
        if(emprunt != null){
            emprunt.Item2.Remove(emprunts);
        }
        else {
            throw new Exception("Emprunt introuvable");
        }
    }
}

public class Library {
    private List<Media> medias = new List<Media>();

    public List<Media> Medias {
        get { return medias; }
        set { medias = value; }
    }

    private Emprunt emprunts = new Emprunt();

    public string this[int reference] {
        get 
        { 
            var media = medias.FirstOrDefault(x => x.Numero_reference == reference);
            if (media != null)
            {
                return media.Titre;
            }
            else
            {
                throw new Exception("Reference introuvable");
            }
        }
        set { medias[reference].Titre = value; }
    }

    public static Library operator +(Library library, Media media){
        library.AjouterMedia(media);
        return library;
    }

    public static Library operator -(Library library, Media media){
        library.RetirerMedia(media);
        return library;
    }

    public void AjouterMedia(Media media){
        medias.Add(media);
    }

    public void RetirerMedia(Media media){
        medias.Remove(media);
    }

    public void EmprunterMedia(Media media){
        if(this[media.Numero_reference] != null) {
            if (media.Nb_exemplaires_dispo > 0)
            {
                emprunts.AjouterEmprunt("client1", media.Titre);
                media.Nb_exemplaires_dispo--;
            }
            else
            {
                throw new Exception("Plus d'exemplaires disponibles");
            }
        }
        else {
            throw new Exception("Reference introuvable");
        }
    }

    public void RetournerMedia(Media media){
        if(this[media.Numero_reference] != null) {
            try {
                emprunts.RetournerEmprunt("client1", media.Titre);
                media.Nb_exemplaires_dispo++;
            }
            catch (Exception e){
                Console.WriteLine(e.Message);    
            }
        }
        else {
            throw new Exception("Reference introuvable");
        }
    }

    public Media RechercheMedia(string titre){
        var media = medias.FirstOrDefault(x => x.Titre == titre);
        if(media != null){
            return media;
        }
        else {
            throw new Exception("Titre introuvable");
        }
    }

    public void AfficherLibrairieStat(){
        Console.WriteLine("Nombre de Media dans la bibliothèque : " + medias.Count());
        Console.WriteLine("Nombre d'exemplaire disponible : " + medias.Sum(x => x.Nb_exemplaires_dispo));
        Console.WriteLine("Nombre d'exemplaire emprunter : " + emprunts.CountEmprunt());
    }

    public void SauvegarderBibliotheque(string nomFichier)
    {
        string json = JsonConvert.SerializeObject(medias);
        File.WriteAllText(nomFichier, json);
    }

    // Méthode pour charger la bibliothèque à partir d'un fichier JSON
    public void ChargerBibliotheque(string nomFichier)
    {
        if (File.Exists(nomFichier))
        {
            string json = File.ReadAllText(nomFichier);
            medias = JsonConvert.DeserializeObject<List<Media>>(json) ?? new List<Media>();
        }
        else
        {
            throw new FileNotFoundException("Le fichier de sauvegarde n'existe pas.");
        }
    }
}


