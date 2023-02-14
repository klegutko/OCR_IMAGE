using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Tesseract;

namespace OCR_IMAGE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                FileNameTextBlock.Text = Path.GetFileName(files[0]);

                try
                {
                    //sciezka do tesseractengine w wersji polskiej (domyslnie przy .exe) jezeli nie znajdzie daje info
                    string appPath = AppDomain.CurrentDomain.BaseDirectory;
                    string tesseractEnginePolPath = appPath + "tesseractPol";
                    if (Directory.Exists(tesseractEnginePolPath))
                    {
                        ProcedujOdczytywanie(files, tesseractEnginePolPath);
                    }
                    else
                        MessageBox.Show($"Nie odnaleziono folderu {tesseractEnginePolPath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd {ex.Message} podczas próby czytania pliku {files[0]}");
                }
            }
        }

        private void ProcedujOdczytywanie(string[] files, string tesseractEnginePolPath)
        {
            using (var engine = new TesseractEngine(tesseractEnginePolPath, "pol", EngineMode.Default))
            {
                // Wczytaj obraz
                using (var img = Pix.LoadFromFile(files[0]))
                {
                    // Wykonaj OCR na obrazie
                    using (var page = engine.Process(img))
                    {
                        // Pobierz tekst z wyniku OCR
                        string text = page.GetText();

                        // Zapisz tekst do okna - czyszcząc puste znaki
                        TekstPliku.Text = text.Trim();

                        //Obraz do okna
                        Obraz.Source = new BitmapImage(new Uri($"{files[0]}"));
                    }
                }
            }
        }
    }
}
