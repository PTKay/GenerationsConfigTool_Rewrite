using ConfigurationTool.Handlers;
using ConfigurationTool.Helpers;
using ConfigurationTool.Model;
using ConfigurationTool.Model.Devices;
using ConfigurationTool.Model.Input;
using ConfigurationTool.Model.Settings;
using ConfigurationTool.Settings.Model;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConfigurationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ConfigurationHandler Files;

        private readonly Configuration Configuration;
        private bool isInitialized = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Files = new ConfigurationHandler();
            this.Configuration = Files.LoadConfiguration();

            // Get Graphics Adapters
            List<GraphicsAdapter> adapters = DevicesHandler.GetGraphicsAdapters();
            this.GPUSelector.ItemsSource = adapters;
            this.ResSelector.ItemsSource = adapters[0].Resolutions;

            // Get Audio Devices
            this.AudioSelector.ItemsSource = DevicesHandler.GetAudioDevices();

            this.FxaaSelector.ItemsSource = Enum.GetValues(typeof(OnOff));
            this.VSyncSelector.ItemsSource = Enum.GetValues(typeof(OnOff));

            this.DispModeSelector.ItemsSource = EnumOrder<DisplayMode>.Values;

            this.ShadowSelector.ItemsSource = Enum.GetValues(typeof(HighLow));
            this.ReflectionSelector.ItemsSource = Enum.GetValues(typeof(HighLow));

            this.LanguageSelector.ItemsSource = EnumOrder<Language>.Values;

            this.InputSelector.Items.Add(this.Configuration.Keyboard);
            bool observed = this.Configuration.XinputController.IsConnected;
            if (observed) this.InputSelector.Items.Add(this.Configuration.XinputController);

            this.InputSelector.SelectedIndex = 0;

            UpdateConfigView(observed, this.Configuration.XinputController);
        }

        private async Task UpdateConfigView(bool isConnected, InputDevice xinput)
        {
            if (this.Configuration.GraphicsAdapter != null && this.GPUSelector.Items.IndexOf(this.Configuration.GraphicsAdapter) >= 0)
            {
                this.GPUSelector.SelectedItem = this.Configuration.GraphicsAdapter;
                this.ResSelector.SelectedItem = this.Configuration.Resolution;
            }
            else
            {
                this.Configuration.GraphicsAdapter = (GraphicsAdapter)this.GPUSelector.Items[0];
                this.Configuration.Resolution = this.Configuration.GraphicsAdapter.Resolutions[0];

                this.GPUSelector.SelectedIndex = 0;
                this.ResSelector.SelectedIndex = 0;
            }

            this.FxaaSelector.SelectedItem = this.Configuration.Antialiasing;
            this.VSyncSelector.SelectedItem = this.Configuration.VSync;
            this.DispModeSelector.SelectedItem = this.Configuration.DisplayMode;
            this.ShadowSelector.SelectedItem = this.Configuration.ShadowQuality;
            this.ReflectionSelector.SelectedItem = this.Configuration.ReflectionQuality;

            int idx = this.AudioSelector.Items.IndexOf(this.Configuration.AudioDevice);
            this.AudioSelector.SelectedIndex = idx < 0 ? 0 : idx;

            this.Analytics_Enabled.IsChecked = this.Configuration.Analytics == OnOff.On;

            this.LanguageSelector.SelectedItem = Configuration.Language;

            if (Configuration.ProcessIsElevated)
            {
                this.LanguageLabel.Opacity = 1;
                this.AdminButton.Visibility = Configuration.ProcessIsElevated ? Visibility.Collapsed : Visibility.Visible;
                this.AdminButtonCol.Width = GridLength.Auto;
            }
            else 
            {
                this.LanguageSelector.Foreground = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
                this.LanguageSelector.IsEnabled = false;
                this.AdminButtonImage.Source = UacHelper.GetUacIcon();
            }

            isInitialized = true;

            // Fill the Input buttons
            FillButtons();

            // Poll the Xbox controller port
            while (true)
            {
                await Task.Run(() =>
                {
                    SpinWait.SpinUntil(() => isConnected != xinput.IsConnected);

                    isConnected = !isConnected;
                });

                if (isConnected)
                {
                    this.InputSelector.Items.Add(xinput);
                }
                else
                {
                    this.InputSelector.SelectedIndex = 0;
                    this.InputSelector.Items.Remove(xinput);
                }
            }
        }

        private void FillButtons()
        {
            this.Button_A.Content = ((Key)this.Configuration.Keyboard.Buttons.A).ToString();
            this.Button_B.Content = ((Key)this.Configuration.Keyboard.Buttons.B).ToString();
            this.Button_X.Content = ((Key)this.Configuration.Keyboard.Buttons.X).ToString();
            this.Button_Y.Content = ((Key)this.Configuration.Keyboard.Buttons.Y).ToString();

            this.Button_RB.Content = ((Key)this.Configuration.Keyboard.Buttons.RB).ToString();
            this.Button_LB.Content = ((Key)this.Configuration.Keyboard.Buttons.LB).ToString();

            this.Button_RT.Content = ((Key)this.Configuration.Keyboard.Buttons.RT).ToString();
            this.Button_LT.Content = ((Key)this.Configuration.Keyboard.Buttons.LT).ToString();

            this.Button_Up.Content = ((Key)this.Configuration.Keyboard.Buttons.Up).ToString();
            this.Button_Down.Content = ((Key)this.Configuration.Keyboard.Buttons.Down).ToString();
            this.Button_Left.Content = ((Key)this.Configuration.Keyboard.Buttons.Left).ToString();
            this.Button_Right.Content = ((Key)this.Configuration.Keyboard.Buttons.Right).ToString();

            this.Button_Start.Content = ((Key)this.Configuration.Keyboard.Buttons.Start).ToString();
            this.Button_Back.Content = ((Key)this.Configuration.Keyboard.Buttons.Back).ToString();
        }

        private async Task UpdateKeyboardKey(Button src)
        {
            int key = -1;

            await Task.Run(() =>
            {
                SpinWait.SpinUntil(() =>
                {
                    key = this.Configuration.Keyboard.GetKey();
                    return key != 0 && key != (int)Key.LeftWindowsKey && key != (int)Key.RightWindowsKey;
                });
            });

            this.ParentGrid.IsEnabled = true;
            if (key != -1)
            {
                FieldInfo targetProperty = null;
                FieldInfo[] props = typeof(ButtonConfiguration).GetFields();

                for (int i = 0; i < props.Length; ++i)
                {
                    if (props[i].Name.Equals(src.Tag.ToString()))
                    {
                        targetProperty = props[i];
                        break;
                    }
                }
                targetProperty?.SetValue(this.Configuration.Keyboard.Buttons, key);
                src.Content = ((Key)key).ToString();
            }
        }

        private void UI_Save_Click(object sender, RoutedEventArgs e)
        {
            Files.SaveConfiguration(this.Configuration);
        }

        private void UI_SaveAndQuit_Click(object sender, RoutedEventArgs e)
        {
            Files.SaveConfiguration(this.Configuration);
            Application.Current.Shutdown();
        }

        private void UI_Quit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void AdminButton_Click(object sender, RoutedEventArgs e) => UacHelper.RestartAsAdmin();

        private void GPUSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("GraphicsAdapter_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void ResSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("Resolution_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Res.png", UriKind.Relative));
        }

        private void FxaaSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("Antialiasing_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/FXAA.png", UriKind.Relative));
        }

        private void DispModeSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("DisplayMode_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Display.png", UriKind.Relative));
        }

        private void ShadowSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("ShadowQuality_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Shadows.png", UriKind.Relative));
        }

        private void ReflectionSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("ReflectionQuality_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void VSyncSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("VSync_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/VSync.png", UriKind.Relative));
        }

        private void AudioSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("AudioDevice_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void LanguageSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Tooltip.Text = Application.Current.TryFindResource("Language_Desc").ToString();
            this.TooltipImage.Source = new BitmapImage(new Uri("Resources/Images/Misc.png", UriKind.Relative));
        }

        private void AnalyticsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                Application.Current.TryFindResource("AnalyticsText").ToString(),
                Application.Current.TryFindResource("AnalyticsInfo").ToString());
        }

        private void GPUSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            GraphicsAdapter adapter = (GraphicsAdapter)e.AddedItems[0];
            this.Configuration.GraphicsAdapter = adapter;
            this.ResSelector.ItemsSource = adapter.Resolutions;
            this.ResSelector.SelectedIndex = 0;
        }

        private void ResSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.Resolution = (Resolution)e.AddedItems[0];
        }

        private void FxaaSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.Antialiasing = (OnOff)e.AddedItems[0];
        }

        private void DispModeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.DisplayMode = (DisplayMode)e.AddedItems[0];
        }

        private void ShadowSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.ShadowQuality = (HighLow)e.AddedItems[0];
        }

        private void ReflectionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.ReflectionQuality = (HighLow)e.AddedItems[0];
        }

        private void VSyncSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.VSync = (OnOff)e.AddedItems[0];
        }

        private void AudioSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.AudioDevice = (AudioDevice)e.AddedItems[0];
        }

        private void Analytics_Enabled_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.Analytics = OnOff.On;
        }

        private void Analytics_Disabled_Click(object sender, RoutedEventArgs e)
        {
            

            this.Configuration.Analytics = OnOff.Off;
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) return;

            this.Configuration.Language = (Language)e.AddedItems[0];
        }

        private void InputSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((InputDevice)e.AddedItems[0]).DeviceType == Model.Devices.DeviceType.XINPUT)
            {
                this.InputButtons.IsEnabled = false;
            }
            else
            {
                this.InputButtons.IsEnabled = true;
            }
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            this.ParentGrid.IsEnabled = false;
            Button src = ((Button)e.Source);

            UpdateKeyboardKey(src);
        }

        private void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            this.Configuration.Keyboard.Buttons = new Model.Input.ButtonConfiguration();
            FillButtons();
        }
    }
}
