using Music_Player.Model;
using Music_Player.Uti;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ListViewItem = System.Windows.Controls.ListViewItem;
using MahApps.Metro.IconPacks;

namespace Music_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    { 
        public bool IsStart = false;
        public bool Play_Puase = false;
        public int index = -1;
        DispatcherTimer timer = new DispatcherTimer();
        public List<MUSIC>? musics { get; private set; }


        public MainWindow()
        {
            InitializeComponent();
            FillData();
            MusicPlayerME.LoadedBehavior = MediaState.Manual;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (MusicPlayerME.NaturalDuration.HasTimeSpan)
            {
                int ali = (int)MusicPlayerME.NaturalDuration.TimeSpan.TotalSeconds;
                int mmd = (int)MusicPlayerME.Position.TotalSeconds;
                time.Text = string.Format("{0:D2}:{1:D2}", mmd / 60, mmd % 60);
                totalTime.Text = string.Format("{0:D2}:{1:D2}", ali / 60, ali % 60);

                silder.Maximum = MusicPlayerME.NaturalDuration.TimeSpan.TotalSeconds;
                silder.Value = MusicPlayerME.Position.TotalSeconds;
            }
        }

        public void FillData()
        {
            Music_List.ItemsSource = musics = DataAccess.GetMusics();
        }
        public void FillData(List<MUSIC>? list = null)
        {
            if (list is null)
            {
                FillData();
            }
            Music_List.ItemsSource = list;

        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SELETE A MUSIC |*.mp3";
            ofd.ShowDialog();
            if(string.IsNullOrEmpty(ofd.FileName))
            {
                DataAccess.AddMusic(ofd.SafeFileName , ofd.FileName);
            }

            FillData();
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem View)
            {
                var mc = Music_List.SelectedItem as MUSIC;
                index = mc.Id;
                MusicPlayerME.Source = new Uri(mc.Path, UriKind.RelativeOrAbsolute);
                MusicPlayerME.Play();
                Play_Puase = true;
                IsStart = true;
                timer.Start();
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (IsStart)
            {
                if (musics.Min(x => x.Id) < index)
                {
                    var mc = musics.OrderByDescending(x => x.Id).First(x => x.Id < index);
                    MusicPlayerME.Source = new Uri(mc.Path, UriKind.RelativeOrAbsolute);
                    index = mc.Id;
                    Music_List.SelectedIndex = Music_List.SelectedIndex + 1;
                    Music_List.SelectedIndex = index;
                    MusicPlayerME.Play();
                    Play_Puase = true;
                }
                else
                {
                    var mc = musics.First(x => x.Id == musics.Max(x => x.Id));
                    MusicPlayerME.Source = new Uri(mc.Path, UriKind.RelativeOrAbsolute);
                    index = mc.Id;
                    MusicPlayerME.Play();
                    Play_Puase = true;
                }
            }
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            PackIconMaterial icon = new PackIconMaterial();

            if (Play_Puase)
            {
                MusicPlayerME.Pause();
                Play_Puase = false;
                icon.Kind = PackIconMaterialKind.Play;
                pause.Content = icon;
            }
            else
            {
                MusicPlayerME.Play();
                Play_Puase = true;
                icon.Kind = PackIconMaterialKind.Pause;
                pause.Content = icon;
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (IsStart)
            {
                if (musics.Max(x => x.Id) > index)
                {
                    var mc = musics.First(x => x.Id > index);
                    MusicPlayerME.Source = new Uri(mc.Path, UriKind.RelativeOrAbsolute);
                    index = mc.Id;
                    MusicPlayerME.Play();
                    Play_Puase = true;
                }
                else
                {
                    var mc = musics.First(x => x.Id == musics.Min(x => x.Id));
                    MusicPlayerME.Source = new Uri(mc.Path, UriKind.RelativeOrAbsolute);
                    index = mc.Id;
                    MusicPlayerME.Play();   
                    Play_Puase = true;
                }
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Music_List.SelectedIndex >= 0)
            {
                var mc = Music_List.SelectedItem as MUSIC;
                DataAccess.DeleteMusic(mc.Id);
            }
            FillData();
        }

        private void OrderAsc_Click(object sender, RoutedEventArgs e)
        {
            var mc = Music_List.ItemsSource is IEnumerable<MUSIC> list ? list.ToList() : new List<MUSIC>();
            mc.Sort(new Comparer12<MUSIC>());
            FillData(mc);
        }

        private void OrderDec_Click(object sender, RoutedEventArgs e)
        {
            var mc = Music_List.ItemsSource is IEnumerable<MUSIC> list ? list.ToList() : new List<MUSIC>();
            mc.Sort(new Comparer12<MUSIC>());
            mc.Reverse();  
            FillData(mc);
        }

        private void silder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MusicPlayerME.Position = TimeSpan.FromSeconds(silder.Value);

            if (MusicPlayerME.NaturalDuration.HasTimeSpan)
            {
                if (MusicPlayerME.Position.TotalSeconds == MusicPlayerME.NaturalDuration.TimeSpan.TotalSeconds)
                {
                    var mc = musics.First(x => x.Id == index);
                    MusicPlayerME.Source = new Uri(mc.Path, UriKind.RelativeOrAbsolute);
                    MusicPlayerME.Play();
                    Play_Puase = true;
                }
            }
        }

        private void VolumeMedium_Click(object sender, RoutedEventArgs e)
        {
            if (vlum.IsVisible)
            {
                vlum.Visibility = Visibility.Collapsed;
            }
            else
            {
                vlum.Visibility = Visibility.Visible;
            }
        }

        private void vlum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (vlum.IsVisible)
            {
                MusicPlayerME.Volume = vlum.Value / 100;
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (MusicPlayerME.SpeedRatio == 1)
            {
                MusicPlayerME.SpeedRatio = 2;
            }
            else
            {
                MusicPlayerME.SpeedRatio = 1;
            }

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!IsLoaded) return;

            string TextSearch = SearchTextBox.Text.Trim();

            var Musics12 = new List<MUSIC>();

            foreach(var Musics in musics)
            {

                var ismatch = Musics.Name.Contains(TextSearch , StringComparison.OrdinalIgnoreCase);

                if(ismatch)
                {
                    Musics12.Add(Musics);
                }
            }
            FillData(Musics12);
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            var mc = musics.First(x => x.Id == index);
            MusicPlayerME.Source = new Uri(mc.Path, UriKind.RelativeOrAbsolute);
            MusicPlayerME.Play();
            Play_Puase = true;
        }
    }
}