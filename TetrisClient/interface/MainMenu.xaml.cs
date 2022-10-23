using System;
using System.Windows;
using System.Windows.Controls;
using TetrisClient.@interface;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        public void ButtonHandler(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            Window window = (string) button.Content switch
            {
                "Single player" => new SinglePlayerWindow(),
                "Multiplayer" => new MultiPlayerWindow(),
                _ => throw new Exception("Invalid option")
            };

            Hide();
            window.Closed += (_, _) => Close();
            window.Show();
        }
    }
}