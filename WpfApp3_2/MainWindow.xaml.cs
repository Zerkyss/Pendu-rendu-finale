using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3_2
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string motADeviner;
        private char[] motCache;
        private int tentativesRestantes;
        private bool jeuGagne;
        private List<char> lettresIncorrectes;

        private string[] mots = {
            "pomme", "banane", "cerise", "datte", "figue", "raisin", "kiwi", "citron", "melon", "orange",
            "abricot", "ananas", "avocat", "framboise", "mangue", "peche", "poire", "prune", "fraise", "raisin",
            "cassis", "grenade", "papaye", "nectarine", "kaki", "myrtille", "amande", "noix", "noisette", "chataigne",
            "carotte", "courgette", "tomate", "brocoli", "concombre", "aubergine", "poivron", "oignon", "ail", "patate",
            "pasteque", "anis", "basilic", "cumin", "coriandre", "thym", "romarin", "laurier", "sauge", "origan",
            "menthe", "persil", "cerfeuil", "ciboulette", "sarriette", "tarragon", "camomille", "melisse", "pissenlit", "verveine",
            "salsifis", "rutabaga", "topinambour", "panais", "cerfeuil", "bette", "moutarde", "radis", "cardon", "roquette",
            "aneth", "fenouil", "endive", "epinard", "poireau", "céleri", "chou", "betterave", "cornichon", "haricot",
            "lentille", "pois", "soja", "feve", "champignon", "girolle", "cèpe", "truffe", "morille", "chanterelle",
            "ble", "maïs", "riz", "orge", "avoine", "seigle", "mil", "sarrasin", "quinoa", "couscous",
            "cochon", "vache", "mouton", "chevre", "ane", "cheval", "lapin", "poule", "canard", "oie",
            "colin", "sardine", "daurade", "carpe", "brochet", "sole", "turbot", "truite", "bar", "thon",
            "chocolat", "vanille", "caramel", "cacao", "miel", "noisette", "amande", "pistache", "cannelle", "gingembre",
            "vanille", "citron", "orange", "citron vert", "citron jaune", "pamplemousse", "mandarine", "kumquat", "yuzu", "bergamote",
            "grenadine", "peche", "abricot", "cassis", "fraise", "framboise", "mure", "myrtille", "raisin", "pomme",
            "banane", "kiwi", "mangue", "ananas", "papaye", "noix de coco", "pasteque", "melon", "pomme grenade",
        };

        public MainWindow()
        {
            InitializeComponent();
            InitialiserJeu();

            KeyDown += Clavier_touche;
        }
        private void Clavier_touche(object sender, KeyEventArgs e)
        {
            // Comparaison "si c'est la fin du game"
            if (jeuGagne | Bouton_Recommencer.Visibility == Visibility.Visible) return;
            char lettre = e.Key.ToString().ToLower()[0];
            Verifier_lettre(lettre);
        }

        private void Verifier_lettre(char lettre)
        {
            if (motADeviner.Contains(lettre))
            {
                for (int i = 0; i < motADeviner.Length; i++)
                {
                    if (motADeviner[i] == lettre)
                    {
                        motCache[i] = lettre;
                    }
                }

                Mot_Adeviner.Text = new string(motCache);

                if (new string(motCache) == motADeviner)
                {
                    jeuGagne = true;
                    Resultat_Jeu.Text = "Félicitations ! Vous avez deviné le mot : " + motADeviner;
                    Bouton_Recommencer.Visibility = Visibility.Visible;
                    Mot_Adeviner.Visibility = Visibility.Hidden;
                    Lettres_Incorrectes.Visibility = Visibility.Hidden;
                }

                Lettre_Saisie.Text = string.Empty;
            }
            else
            {
                tentativesRestantes--;
                Tentatives_Restantes.Text = "Tentatives restantes : " + tentativesRestantes;
                lettresIncorrectes.Add(lettre);
                Lettres_Incorrectes.Text = "Lettres incorrectes : " + string.Join(", ", lettresIncorrectes);

                if (tentativesRestantes == 0)
                {
                    Mot_Adeviner.Visibility = Visibility.Hidden;
                    Resultat_Jeu.Text = "Perdu ! Le mot était : " + motADeviner;
                    foreach (Button btn in Grid_Alphabelt.Children.OfType<Button>())
                    {
                        btn.IsEnabled = false;
                    }
                    Bouton_Recommencer.Visibility = Visibility.Visible;
                }

                Lettre_Saisie.Text = string.Empty;
            }
        }
        private void InitialiserJeu()
        {
            Lettres_Incorrectes.Visibility = Visibility.Visible;
            Mot_Adeviner.Visibility = Visibility.Visible;
            motADeviner = ChoisirMotAleatoire(mots);
            motCache = new char[motADeviner.Length];
            tentativesRestantes = 6;
            jeuGagne = false;
            lettresIncorrectes = new List<char>();
            Resultat_Jeu.Text = "";


            foreach (Button btn in Grid_Alphabelt.Children.OfType<Button>())
            {
                btn.IsEnabled = true;
            }

            for (int i = 0; i < motCache.Length; i++)
            {
                motCache[i] = '-';
            }

            Mot_Adeviner.Text = new string(motCache);
            Tentatives_Restantes.Text = "Tentatives restantes : " + tentativesRestantes;
            Lettres_Incorrectes.Text = "Lettres incorrectes : ";
        }

        private void BoutonDevine_Click(object sender, RoutedEventArgs e)
        {
            if (!jeuGagne && tentativesRestantes > 0)
            {
                char lettre = ' ';

                if (!string.IsNullOrEmpty(Lettre_Saisie.Text))
                {
                    lettre = Lettre_Saisie.Text.ToLower()[0];
                }
                Verifier_lettre(lettre);
            }
        }

        private void BoutonRecommencer_Click(object sender, RoutedEventArgs e)
        {
            InitialiserJeu();
            Bouton_Recommencer.Visibility = Visibility.Hidden;
        }

        private string ChoisirMotAleatoire(string[] mots)
        {
            Random random = new Random();
            int index = random.Next(0, mots.Length);
            return mots[index];
        }

        private void Bouton_Click(object sender, RoutedEventArgs e)
        {
            Button lettre_Bouton = sender as Button;
            char lettre = lettre_Bouton.Content.ToString().ToLower()[0];
            Verifier_lettre(lettre);
        }
    }
}