using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CryptoApp
{
    public partial class MainWindow : Window
    {
        //main data for web request
        static string key = "<YOUR KEY>"; //use your key or uncomment the option without a key
        string url = $"https://api.coincap.io/v2/assets"; //main url
        string imageUrl = "https://coinicons-api.vercel.app/api/icon/"; //url for icons

        //colors, which using for styling (also for light/dark themes)
        SolidColorBrush TopTabSymbolColor = Brushes.LightGray;
        SolidColorBrush TopTabBorderRowColor = Brushes.Gray;

        SolidColorBrush DetailsTabExpanderFontColor = Brushes.WhiteSmoke;
        SolidColorBrush DetailsTabExpanderBackColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
        SolidColorBrush DetailsTabExpanderRankBorderColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
        SolidColorBrush DetailsTabExpanderBorderColor = Brushes.LightGray;
        SolidColorBrush DetailsTabExpDataBackColor = new SolidColorBrush(Color.FromRgb(32, 32, 32));
        SolidColorBrush DetailsTabExpDataBaseFontColor = Brushes.WhiteSmoke;
        SolidColorBrush DetailsTabExpDataSymbolFontColor = Brushes.Gray;
        SolidColorBrush DetailsTabExpDetailsColor = Brushes.LightGray;
        SolidColorBrush DetailsTabExpDetailsDataColor = Brushes.WhiteSmoke;
        SolidColorBrush DetailsTabExpDetailsBorderColor = Brushes.LightGray;

        SolidColorBrush DetailsTabExpMarketsBackColor = new SolidColorBrush(Color.FromRgb(21, 21, 21));
        SolidColorBrush DetailsTabExpMarketsTitleBackColor = new SolidColorBrush(Color.FromRgb(49, 49, 49)); 
        SolidColorBrush DetailsTabExpMarketsPairColor = Brushes.LightGray;
        SolidColorBrush DetailsTabExpMarketsBorderColor = Brushes.Gray;

        public MainWindow()
        {
            InitializeComponent();

            //using our methods for build tabs
            SetTopTabData(Convert.ToInt32(countOfCoisCmbBox.Text));

            SetDetailedCoinsTabData(DetailedCoinsTabData(), SearchTextBox.Text);
            SetExchangeTabData();

            //timer for updating info
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        //method for get info about crypto
        private CryptoResponse WebRequestData()
        {
            //repeat operation as in constructor
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{url}?limit=100&?key={key}");

            //uncomment next row to use without key
            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{url}?limit=100"); 


            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            CryptoResponse cryptoResponse = JsonConvert.DeserializeObject<CryptoResponse>(response);

            return cryptoResponse;
        }

        //timer Tick method (update first tab info per 5 seconds)
        private void timer_Tick(object sender, EventArgs e)
        {
            //delete elements for updating
            TopCurrencyGrid.Children.Clear();
            TopCurrencyGrid.RowDefinitions.Clear();
            SetTopTabData(Convert.ToInt32(countOfCoisCmbBox.Text));
        }

        //Method for filling firs tab with top crypto
        //We just create the elements and add them into container
        private void SetTopTabData( int countOfCoins)
        {
            CryptoResponse cryptoResponse = WebRequestData();

            int rowIndex = 0;
            foreach (var crypto in cryptoResponse.data)
            {
                if (countOfCoins == rowIndex) break;
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(40);
                TopCurrencyGrid.RowDefinitions.Add(row);

                TextBlock rankTextBlock = new TextBlock();
                rankTextBlock.Text = crypto.rank;
                Grid.SetRow(rankTextBlock, rowIndex);
                Grid.SetColumn(rankTextBlock, 0);
                rankTextBlock.FontSize = 12;
                rankTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                rankTextBlock.VerticalAlignment = VerticalAlignment.Center;
                rankTextBlock.FontWeight = FontWeights.Bold;

                Image coinImage = new Image();
                Grid.SetRow(coinImage, rowIndex);
                Grid.SetColumn(coinImage, 1);
                coinImage.Width = 20; 
                coinImage.Source = new BitmapImage(new Uri($@"{imageUrl}{crypto.symbol.ToLower()}", UriKind.RelativeOrAbsolute));

                TextBlock nameTextBlock = new TextBlock();
                nameTextBlock.Text = crypto.name;
                Grid.SetRow(nameTextBlock, rowIndex);
                Grid.SetColumn(nameTextBlock, 2);
                nameTextBlock.FontSize = 12;
                nameTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                nameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                nameTextBlock.FontWeight = FontWeights.Bold;


                TextBlock symbolTextBlock = new TextBlock();
                symbolTextBlock.Text = crypto.symbol;
                Grid.SetRow(symbolTextBlock, rowIndex);
                Grid.SetColumn(symbolTextBlock, 3);
                symbolTextBlock.FontSize = 11;
                symbolTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                symbolTextBlock.VerticalAlignment = VerticalAlignment.Center;
                symbolTextBlock.FontWeight = FontWeights.Bold;
                symbolTextBlock.Foreground = TopTabSymbolColor;

                TextBlock priceTextBlock = new TextBlock();
                priceTextBlock.Text = $"{Price(crypto.priceUsd)}$";
                Grid.SetRow(priceTextBlock, rowIndex);
                Grid.SetColumn(priceTextBlock, 4);
                priceTextBlock.FontSize = 12;
                priceTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                priceTextBlock.VerticalAlignment = VerticalAlignment.Center;
                priceTextBlock.FontWeight = FontWeights.Bold;
                priceTextBlock.Margin = new Thickness(0, 0, 15, 0);

                TextBlock changePercentTextBlock = new TextBlock();
                changePercentTextBlock.Text = $"{Math.Round(Convert.ToDecimal(crypto.changePercent24Hr), 2)}%";
                Grid.SetRow(changePercentTextBlock, rowIndex);
                Grid.SetColumn(changePercentTextBlock, 5);
                changePercentTextBlock.FontSize = 12;
                changePercentTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                changePercentTextBlock.VerticalAlignment = VerticalAlignment.Center;
                changePercentTextBlock.Margin = new Thickness(0, 0, 15, 0);
                if (Convert.ToDecimal(crypto.changePercent24Hr) < 0) changePercentTextBlock.Foreground = Brushes.Red;
                else changePercentTextBlock.Foreground = Brushes.Green;

                Border border = new Border();
                Grid.SetRow(border, rowIndex);
                Grid.SetColumnSpan(border, 6);
                border.BorderBrush = TopTabBorderRowColor;
                border.BorderThickness = new Thickness(0, 0.3, 0, 0);

                TopCurrencyGrid.Children.Add(rankTextBlock);
                TopCurrencyGrid.Children.Add(coinImage);
                TopCurrencyGrid.Children.Add(nameTextBlock);
                TopCurrencyGrid.Children.Add(symbolTextBlock);
                TopCurrencyGrid.Children.Add(priceTextBlock);
                TopCurrencyGrid.Children.Add(border);
                TopCurrencyGrid.Children.Add(changePercentTextBlock);

                rowIndex++;
            }
        }

        //method for changing coins count on page
        //we just recreate the elements and add them into container
        private void countOfCoisCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TopCurrencyGrid != null)
            {
                ComboBoxItem comboBoxItem = (ComboBoxItem)countOfCoisCmbBox.SelectedItem;
                string countOfCoins = comboBoxItem.Content.ToString();

                TopCurrencyGrid.Children.Clear();
                TopCurrencyGrid.RowDefinitions.Clear();
                SetTopTabData(Convert.ToInt32(countOfCoins));
            }
        }

        //Method for filling second tab with ability to view details about crypto
        private Border[] DetailedCoinsTabData()
        {
            if (!isLight) SetLightColours();
            else SetDarkColours();

            CryptoResponse cryptoResponse = WebRequestData();

            Border[] border = new Border[cryptoResponse.data.Length];
            int index = 0;
            foreach (var crypto in cryptoResponse.data)
            {
                Expander expander = new Expander();
                expander.HorizontalAlignment = HorizontalAlignment.Stretch;
                expander.Background = DetailsTabExpanderBackColor;
                expander.Foreground = DetailsTabExpanderFontColor;
                expander.FontWeight = FontWeights.Bold;
                expander.FontSize = 14;
                expander.Tag = $"{crypto.id}";
                expander.Expanded += Expander_Expanded;
                StackPanel header = new StackPanel();
                header.Orientation = Orientation.Horizontal;

                TextBlock HeaderRankTextBox = new TextBlock();
                HeaderRankTextBox.Text = $"#{crypto.rank}";
                HeaderRankTextBox.FontSize = 16;
                HeaderRankTextBox.HorizontalAlignment = HorizontalAlignment.Center;
                HeaderRankTextBox.VerticalAlignment = VerticalAlignment.Center;
                HeaderRankTextBox.FontWeight = FontWeights.Bold;

                Image HeaderCoinImage = new Image();
                HeaderCoinImage.Width = 25;
                HeaderCoinImage.Margin = new Thickness(10, 0, 10, 0);
                HeaderCoinImage.Source = new BitmapImage(new Uri($@"{imageUrl}{crypto.symbol.ToLower()}", UriKind.RelativeOrAbsolute));

                TextBlock HeaderNameTextBlock = new TextBlock();
                HeaderNameTextBlock.Text = crypto.name;
                HeaderNameTextBlock.FontSize = 16;
                HeaderNameTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                HeaderNameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                HeaderNameTextBlock.FontWeight = FontWeights.Bold;


                TextBlock HeaderSymbolTextBlock = new TextBlock();
                HeaderSymbolTextBlock.Text = $"({crypto.symbol})";
                HeaderSymbolTextBlock.FontSize = 16;
                HeaderSymbolTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                HeaderSymbolTextBlock.VerticalAlignment = VerticalAlignment.Center;
                HeaderSymbolTextBlock.FontWeight = FontWeights.Bold;
                HeaderSymbolTextBlock.Margin = new Thickness(10, 0, 10, 0);

                header.Children.Add(HeaderRankTextBox);
                header.Children.Add(HeaderCoinImage);
                header.Children.Add(HeaderNameTextBlock);
                header.Children.Add(HeaderSymbolTextBlock);

                expander.Header = header;

                border[index] = new Border();
                border[index].BorderBrush = DetailsTabExpanderBorderColor;
                border[index].BorderThickness = new Thickness(0, 0, 0, 0.5);
                border[index].Child = expander;
                border[index].Tag = $"{crypto.name} {crypto.symbol}";
                index++;
            }
            return border;
        }

        //Method for filling details info about unique crypto if expaner got focus
        //this made for optimize program
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Expander expander = (Expander)sender;

            CryptoResponse cryptoResponse = WebRequestData();

            CryptoData crypto = new CryptoData();
            foreach(var coin in cryptoResponse.data)
            {
                if (coin.id == expander.Tag.ToString()) crypto = coin;
            }

            StackPanel mainStackPanel = new StackPanel();
            mainStackPanel.Orientation = Orientation.Horizontal;
            mainStackPanel.Background = DetailsTabExpDataBackColor;

            StackPanel detailsStackPanel = new StackPanel();
            detailsStackPanel.Margin = new Thickness(0, 0, 0, 10);

            Border rankBorder = new Border();
            rankBorder.Background = DetailsTabExpanderRankBorderColor;
            rankBorder.HorizontalAlignment = HorizontalAlignment.Left;
            rankBorder.Margin = new Thickness(10, 5, 10, 5);
            rankBorder.CornerRadius = new CornerRadius(10);

            TextBlock rankTextBlock = new TextBlock();
            rankTextBlock.Text = $"Rank #{crypto.rank}";
            rankTextBlock.Margin = new Thickness(5);

            rankBorder.Child = rankTextBlock;

            StackPanel TitleStackPanel = new StackPanel();
            TitleStackPanel.Orientation = Orientation.Horizontal;
            TitleStackPanel.Margin = new Thickness(10, 0, 10, 0);

            Image TitleCoinImage = new Image();
            TitleCoinImage.Width = 35;
            TitleCoinImage.Margin = new Thickness(10, 0, 10, 0);
            TitleCoinImage.Source = new BitmapImage(new Uri($@"{imageUrl}{crypto.symbol.ToLower()}", UriKind.Absolute));

            TextBlock TitleNameTextBlock = new TextBlock();
            TitleNameTextBlock.Text = crypto.name;
            TitleNameTextBlock.FontSize = 18;
            TitleNameTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            TitleNameTextBlock.VerticalAlignment = VerticalAlignment.Center;
            TitleNameTextBlock.FontWeight = FontWeights.Bold;
            TitleNameTextBlock.Foreground = DetailsTabExpDataBaseFontColor;


            TextBlock TitleSymbolTextBlock = new TextBlock();
            TitleSymbolTextBlock.Text = crypto.symbol;
            TitleSymbolTextBlock.FontSize = 15;
            TitleSymbolTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            TitleSymbolTextBlock.VerticalAlignment = VerticalAlignment.Center;
            TitleSymbolTextBlock.FontWeight = FontWeights.Bold;
            TitleSymbolTextBlock.Foreground = DetailsTabExpDataSymbolFontColor;
            TitleSymbolTextBlock.Margin = new Thickness(10, 3, 0, 0);

            TitleStackPanel.Children.Add(TitleCoinImage);
            TitleStackPanel.Children.Add(TitleNameTextBlock);
            TitleStackPanel.Children.Add(TitleSymbolTextBlock);

            StackPanel PriceStackPanel = new StackPanel();
            PriceStackPanel.Orientation = Orientation.Horizontal;
            PriceStackPanel.Margin = new Thickness(10, 5, 10, 5);

            TextBlock PriceTextBlock = new TextBlock();
            PriceTextBlock.Text = $"{Price(crypto.priceUsd)}$";
            PriceTextBlock.FontSize = 20;
            PriceTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            PriceTextBlock.VerticalAlignment = VerticalAlignment.Center;
            PriceTextBlock.FontWeight = FontWeights.Bold;
            PriceTextBlock.Foreground = DetailsTabExpDataBaseFontColor;

            TextBlock ChngTextBlock = new TextBlock();
            ChngTextBlock.Text = $"{Math.Round(Convert.ToDecimal(crypto.changePercent24Hr), 2)}%";
            ChngTextBlock.FontSize = 16;
            ChngTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            ChngTextBlock.VerticalAlignment = VerticalAlignment.Center;
            ChngTextBlock.FontWeight = FontWeights.Bold;
            if (Convert.ToDecimal(crypto.changePercent24Hr) < 0) ChngTextBlock.Foreground = Brushes.Red;
            else ChngTextBlock.Foreground = Brushes.Green;
            ChngTextBlock.Margin = new Thickness(10, 5, 10, 5);

            PriceStackPanel.Children.Add(PriceTextBlock);
            PriceStackPanel.Children.Add(ChngTextBlock);

            DockPanel detailsAvgDockPanel = new DockPanel();
            detailsAvgDockPanel.Margin = new Thickness(5, 2, 5, 2);

            TextBlock AvgPriceTextBlock = new TextBlock();
            AvgPriceTextBlock.Text = "24h average price:";
            AvgPriceTextBlock.FontSize = 14;
            AvgPriceTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            AvgPriceTextBlock.VerticalAlignment = VerticalAlignment.Center;
            AvgPriceTextBlock.FontWeight = FontWeights.Light;
            AvgPriceTextBlock.Foreground = DetailsTabExpDetailsColor;

            TextBlock AvgPriceDataTextBlock = new TextBlock();
            AvgPriceDataTextBlock.Text = $"{Math.Round(Convert.ToDecimal(crypto.vwap24Hr), 2)}$";
            AvgPriceDataTextBlock.FontSize = 14;
            AvgPriceDataTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            AvgPriceDataTextBlock.VerticalAlignment = VerticalAlignment.Center;
            AvgPriceDataTextBlock.FontWeight = FontWeights.Light;
            AvgPriceDataTextBlock.Foreground = DetailsTabExpDetailsDataColor;
            AvgPriceDataTextBlock.Margin = new Thickness(10, 0, 0, 0);

            detailsAvgDockPanel.Children.Add(AvgPriceTextBlock);
            detailsAvgDockPanel.Children.Add(AvgPriceDataTextBlock);

            DockPanel detailsVolDockPanel = new DockPanel();
            detailsVolDockPanel.Margin = new Thickness(5, 2, 5, 2);

            TextBlock VolPriceTextBlock = new TextBlock();
            VolPriceTextBlock.Text = "24h USD volume:";
            VolPriceTextBlock.FontSize = 14;
            VolPriceTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            VolPriceTextBlock.VerticalAlignment = VerticalAlignment.Center;
            VolPriceTextBlock.FontWeight = FontWeights.Light;
            VolPriceTextBlock.Foreground = DetailsTabExpDetailsColor;

            TextBlock VolPriceDataTextBlock = new TextBlock();
            VolPriceDataTextBlock.Text = $"{Math.Round(Convert.ToDecimal(crypto.volumeUsd24Hr), 2)}$";
            VolPriceDataTextBlock.FontSize = 14;
            VolPriceDataTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            VolPriceDataTextBlock.VerticalAlignment = VerticalAlignment.Center;
            VolPriceDataTextBlock.FontWeight = FontWeights.Light;
            VolPriceDataTextBlock.Foreground = DetailsTabExpDetailsDataColor;
            VolPriceDataTextBlock.Margin = new Thickness(10, 0, 0, 0);

            detailsVolDockPanel.Children.Add(VolPriceTextBlock);
            detailsVolDockPanel.Children.Add(VolPriceDataTextBlock);

            DockPanel detailsMaxSupDockPanel = new DockPanel();
            detailsMaxSupDockPanel.Margin = new Thickness(5, 2, 5, 2);

            TextBlock MaxSupPriceTextBlock = new TextBlock();
            MaxSupPriceTextBlock.Text = "Max supply:";
            MaxSupPriceTextBlock.FontSize = 14;
            MaxSupPriceTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            MaxSupPriceTextBlock.VerticalAlignment = VerticalAlignment.Center;
            MaxSupPriceTextBlock.FontWeight = FontWeights.Light;
            MaxSupPriceTextBlock.Foreground = DetailsTabExpDetailsColor;

            TextBlock MaxSupPriceDataTextBlock = new TextBlock();
            MaxSupPriceDataTextBlock.Text = $"{Math.Round(Convert.ToDecimal(crypto.maxSupply), 2)}";
            MaxSupPriceDataTextBlock.FontSize = 14;
            MaxSupPriceDataTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            MaxSupPriceDataTextBlock.VerticalAlignment = VerticalAlignment.Center;
            MaxSupPriceDataTextBlock.FontWeight = FontWeights.Light;
            MaxSupPriceDataTextBlock.Foreground = DetailsTabExpDetailsDataColor;
            MaxSupPriceDataTextBlock.Margin = new Thickness(10, 0, 0, 0);

            detailsMaxSupDockPanel.Children.Add(MaxSupPriceTextBlock);
            detailsMaxSupDockPanel.Children.Add(MaxSupPriceDataTextBlock);

            DockPanel detailsTotSupDockPanel = new DockPanel();
            detailsTotSupDockPanel.Margin = new Thickness(5, 2, 5, 2);

            TextBlock TotSupPriceTextBlock = new TextBlock();
            TotSupPriceTextBlock.Text = "Total supply:";
            TotSupPriceTextBlock.FontSize = 14;
            TotSupPriceTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            TotSupPriceTextBlock.VerticalAlignment = VerticalAlignment.Center;
            TotSupPriceTextBlock.FontWeight = FontWeights.Light;
            TotSupPriceTextBlock.Foreground = DetailsTabExpDetailsColor;

            TextBlock TotSupPriceDataTextBlock = new TextBlock();
            TotSupPriceDataTextBlock.Text = $"{Math.Round(Convert.ToDecimal(crypto.supply), 2)}";
            TotSupPriceDataTextBlock.FontSize = 14;
            TotSupPriceDataTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            TotSupPriceDataTextBlock.VerticalAlignment = VerticalAlignment.Center;
            TotSupPriceDataTextBlock.FontWeight = FontWeights.Light;
            TotSupPriceDataTextBlock.Foreground = DetailsTabExpDetailsDataColor;
            TotSupPriceDataTextBlock.Margin = new Thickness(10, 0, 0, 0);

            detailsTotSupDockPanel.Children.Add(TotSupPriceTextBlock);
            detailsTotSupDockPanel.Children.Add(TotSupPriceDataTextBlock);

            Border borderAvgDockPanels = new Border();
            borderAvgDockPanels.BorderBrush = DetailsTabExpDetailsBorderColor;
            borderAvgDockPanels.BorderThickness = new Thickness(0, 0, 0, 0.2);
            borderAvgDockPanels.Margin = new Thickness(5);

            Border borderVolDockPanels = new Border();
            borderVolDockPanels.BorderBrush = DetailsTabExpDetailsBorderColor;
            borderVolDockPanels.BorderThickness = new Thickness(0, 0, 0, 0.2);
            borderVolDockPanels.Margin = new Thickness(5);

            Border borderMaxSupDockPanels = new Border();
            borderMaxSupDockPanels.BorderBrush = DetailsTabExpDetailsBorderColor;
            borderMaxSupDockPanels.BorderThickness = new Thickness(0, 0, 0, 0.2);
            borderMaxSupDockPanels.Margin = new Thickness(5);

            Border borderTotSupDockPanels = new Border();
            borderTotSupDockPanels.BorderBrush = DetailsTabExpDetailsBorderColor;
            borderTotSupDockPanels.BorderThickness = new Thickness(0, 0, 0, 0.2);
            borderTotSupDockPanels.Margin = new Thickness(5);


            detailsStackPanel.Children.Add(rankBorder);
            detailsStackPanel.Children.Add(TitleStackPanel);
            detailsStackPanel.Children.Add(PriceStackPanel);

            borderAvgDockPanels.Child = detailsAvgDockPanel;
            detailsStackPanel.Children.Add(borderAvgDockPanels);


            borderVolDockPanels.Child = detailsVolDockPanel;
            detailsStackPanel.Children.Add(borderVolDockPanels);

            borderMaxSupDockPanels.Child = detailsMaxSupDockPanel;
            detailsStackPanel.Children.Add(borderMaxSupDockPanels);

            borderTotSupDockPanels.Child = detailsTotSupDockPanel;
            detailsStackPanel.Children.Add(borderTotSupDockPanels);

            StackPanel marketsStackPanel = new StackPanel();
            marketsStackPanel.Margin = new Thickness(10, 5, 20, 5);


            TextBlock TitleTextBlock = new TextBlock();
            TitleTextBlock.Text = "Markets";
            TitleTextBlock.Foreground = DetailsTabExpDataBaseFontColor;
            TitleTextBlock.FontWeight = FontWeights.Bold;
            TitleTextBlock.FontSize = 18;
            TitleTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            TitleTextBlock.VerticalAlignment = VerticalAlignment.Center;
            TitleTextBlock.Margin = new Thickness(10, 0, 10, 0);

            Grid MarketsTitleGrid = new Grid();
            MarketsTitleGrid.Height = 40;
            MarketsTitleGrid.Background = DetailsTabExpMarketsTitleBackColor;

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(30);
            MarketsTitleGrid.ColumnDefinitions.Add(column1);

            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(90);
            MarketsTitleGrid.ColumnDefinitions.Add(column2);

            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = new GridLength(90);
            MarketsTitleGrid.ColumnDefinitions.Add(column3);

            ColumnDefinition column4 = new ColumnDefinition();
            column4.Width = new GridLength(95);
            MarketsTitleGrid.ColumnDefinitions.Add(column4);

            TextBlock marketRankTextBlock = new TextBlock();
            marketRankTextBlock.Text = "#";
            marketRankTextBlock.FontSize = 14;
            marketRankTextBlock.FontWeight = FontWeights.Bold;
            marketRankTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            marketRankTextBlock.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(marketRankTextBlock, 0);

            TextBlock marketExchangeTextBlock = new TextBlock();
            marketExchangeTextBlock.Text = "Exchange";
            marketExchangeTextBlock.FontSize = 14;
            marketExchangeTextBlock.FontWeight = FontWeights.Bold;
            marketExchangeTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            marketExchangeTextBlock.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(marketExchangeTextBlock, 1);

            TextBlock marketPairTextBlock = new TextBlock();
            marketPairTextBlock.Text = "Pair";
            marketPairTextBlock.FontSize = 14;
            marketPairTextBlock.FontWeight = FontWeights.Bold;
            marketPairTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            marketPairTextBlock.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(marketPairTextBlock, 2);

            TextBlock marketPriceTextBlock = new TextBlock();
            marketPriceTextBlock.Text = "Price";
            marketPriceTextBlock.FontSize = 14;
            marketPriceTextBlock.FontWeight = FontWeights.Bold;
            marketPriceTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            marketPriceTextBlock.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(marketPriceTextBlock, 3);

            MarketsTitleGrid.Children.Add(marketRankTextBlock);
            MarketsTitleGrid.Children.Add(marketExchangeTextBlock);
            MarketsTitleGrid.Children.Add(marketPairTextBlock);
            MarketsTitleGrid.Children.Add(marketPriceTextBlock);

            ScrollViewer marketsScrollViewer = new ScrollViewer();
            marketsScrollViewer.CanContentScroll = true;
            marketsScrollViewer.Height = 185;

            Grid MarketsGrid = new Grid();
            MarketsGrid.Background = DetailsTabExpMarketsBackColor;

            ColumnDefinition column1_1 = new ColumnDefinition();
            column1_1.Width = new GridLength(30);
            MarketsGrid.ColumnDefinitions.Add(column1_1);

            ColumnDefinition column1_2 = new ColumnDefinition();
            column1_2.Width = new GridLength(90);
            MarketsGrid.ColumnDefinitions.Add(column1_2);

            ColumnDefinition column1_3 = new ColumnDefinition();
            column1_3.Width = new GridLength(90);
            MarketsGrid.ColumnDefinitions.Add(column1_3);

            ColumnDefinition column1_4 = new ColumnDefinition();
            column1_4.Width = new GridLength(95);
            MarketsGrid.ColumnDefinitions.Add(column1_4);

            SetMarketsData(MarketsGrid, crypto.id);

            marketsScrollViewer.Content = MarketsGrid;

            marketsStackPanel.Children.Add(TitleTextBlock);
            marketsStackPanel.Children.Add(MarketsTitleGrid);
            marketsStackPanel.Children.Add(marketsScrollViewer);

            mainStackPanel.Children.Add(detailsStackPanel);
            mainStackPanel.Children.Add(marketsStackPanel);
            expander.Content = mainStackPanel;
        }
        //method for delitting elements, if expander lost focus
        private void Expander_LostFocus(object sender, RoutedEventArgs e)
        {
            Expander expander = (Expander)sender;
            expander.Content = null;
        }

        //method which used in DetailedCoinsTabData method
        //with its, markets data are created
        private void SetMarketsData(Grid marketGrid, string coinName)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{url}/{coinName}/markets?limit=25&?key={key}");

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            MarketsResponse marketsResponse = JsonConvert.DeserializeObject<MarketsResponse>(response);

            int rowIndex = 0;
            foreach (var market in marketsResponse.data)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(30);
                marketGrid.RowDefinitions.Add(row);

                TextBlock rankTextBlock = new TextBlock();
                rankTextBlock.Text = (rowIndex+1).ToString();
                Grid.SetRow(rankTextBlock, rowIndex);
                Grid.SetColumn(rankTextBlock, 0);
                rankTextBlock.FontSize = 12;
                rankTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                rankTextBlock.VerticalAlignment = VerticalAlignment.Center;
                rankTextBlock.FontWeight = FontWeights.Bold;

                TextBlock exchangeTextBlock = new TextBlock();
                exchangeTextBlock.Text = market.exchangeId;
                Grid.SetRow(exchangeTextBlock, rowIndex);
                Grid.SetColumn(exchangeTextBlock, 1);
                exchangeTextBlock.FontSize = 12;
                exchangeTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                exchangeTextBlock.VerticalAlignment = VerticalAlignment.Center;
                exchangeTextBlock.FontWeight = FontWeights.Bold;


                TextBlock pairTextBlock = new TextBlock();
                pairTextBlock.Text = $"{market.baseSymbol}/{market.quoteSymbol}";
                Grid.SetRow(pairTextBlock, rowIndex);
                Grid.SetColumn(pairTextBlock, 2);
                pairTextBlock.FontSize = 12;
                pairTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                pairTextBlock.VerticalAlignment = VerticalAlignment.Center;
                pairTextBlock.FontWeight = FontWeights.Bold;
                pairTextBlock.Foreground = DetailsTabExpMarketsPairColor;

                TextBlock priceTextBlock = new TextBlock();
                priceTextBlock.Text = $"{Math.Round(Convert.ToDecimal(market.priceUsd), 2)}$";
                Grid.SetRow(priceTextBlock, rowIndex);
                Grid.SetColumn(priceTextBlock, 3);
                priceTextBlock.FontSize = 12;
                priceTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                priceTextBlock.VerticalAlignment = VerticalAlignment.Center;
                priceTextBlock.FontWeight = FontWeights.Bold;

                Border border = new Border();
                Grid.SetRow(border, rowIndex);
                Grid.SetColumnSpan(border, 4);
                border.BorderBrush = DetailsTabExpMarketsBorderColor;
                border.BorderThickness = new Thickness(0, 0.2, 0, 0);

                marketGrid.Children.Add(rankTextBlock);
                marketGrid.Children.Add(exchangeTextBlock);
                marketGrid.Children.Add(pairTextBlock);
                marketGrid.Children.Add(priceTextBlock);
                marketGrid.Children.Add(border);
                rowIndex++;
            }
        }

        //method to add data on the second tab (also using for search coins)
        //using array which created by DetailedCoinsTabData method
        private void SetDetailedCoinsTabData(Border[] borders, string search = "")
        {
            DetailedCoinsStackPanel.Children.Clear();
            foreach (Border border in borders)
            {
                string coinData = (string)border.Tag;
                if (coinData.ToLower().Contains(search.ToLower()))
                {
                    DetailedCoinsStackPanel.Children.Add(border);
                }
            }
        }

        //search method (second tab)
        //using SetDetailedCoinsTabData method to find needed elements
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DetailedCoinsStackPanel.Children.Clear();
            SetDetailedCoinsTabData(DetailedCoinsTabData(), SearchTextBox.Text);
        }

        //variable which show what theme using now
        bool isLight = true;
       

        //Method for dark/light themes. Set needed colors for all elements (by methods) and recreate elements :(
        //Maybe the better way it's creating resourses with ready styles
        private void SetDarkColours()
        {
            DetailsTabExpanderFontColor = Brushes.WhiteSmoke;
            DetailsTabExpanderBackColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            DetailsTabExpanderRankBorderColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            DetailsTabExpanderBorderColor = Brushes.LightGray;
            DetailsTabExpDataBackColor = new SolidColorBrush(Color.FromRgb(32, 32, 32));
            DetailsTabExpDataBaseFontColor = Brushes.WhiteSmoke;
            DetailsTabExpDataSymbolFontColor = Brushes.Gray;
            DetailsTabExpDetailsColor = Brushes.LightGray;
            DetailsTabExpDetailsDataColor = Brushes.WhiteSmoke;
            DetailsTabExpDetailsBorderColor = Brushes.LightGray;
                        
            DetailsTabExpMarketsBackColor = new SolidColorBrush(Color.FromRgb(21, 21, 21));
            DetailsTabExpMarketsTitleBackColor = new SolidColorBrush(Color.FromRgb(49, 49, 49));
            DetailsTabExpMarketsPairColor = Brushes.LightGray;
            DetailsTabExpMarketsBorderColor = Brushes.Gray;

            MainStackPanel.Background = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            CoinsInfoTextBlock.Foreground = Brushes.LightGray;
            MarketsInfoTextBlock.Foreground = Brushes.LightGray;
            CoinsInfoDataTextBlock.Foreground = Brushes.Khaki;
            MarketsInfoDataTextBlock.Foreground = Brushes.Khaki;
            LanguageThemeButton.Background = Brushes.LightGray;
            LanguageThemeButton.Foreground = new SolidColorBrush(Color.FromRgb(35, 35, 35));

            MainCard.Background = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetBackground(MainTabControl, new SolidColorBrush(Color.FromRgb(35, 35, 35)));
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetForeground(MainTabControl, Brushes.White);
            TopTab.Foreground = Brushes.LightGray;
            countOfCoisCmbBox.Background = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            countOfCoisCmbBox.BorderBrush = Brushes.LightGray;
            MaterialDesignThemes.Wpf.ThemeAssist.SetTheme(countOfCoisCmbBox, BaseTheme.Dark);
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(countOfCoisCmbBox, Brushes.White);
            TitleGrid.Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));

            DetailsTab.Background = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            SearchStackPanel.Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
            SearchTextBlock.Foreground = Brushes.LightGray;
            SearchTextBox.Foreground = Brushes.LightGray;
            SearchTextBox.BorderBrush = Brushes.LightGray;
            SearchTextBox.Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            SearchTextBox.CaretBrush = Brushes.White;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(SearchTextBox, Brushes.LightGray);


            TopTabSymbolColor = Brushes.LightGray;
            TopTabBorderRowColor = Brushes.Gray;

            ExchangeTab.Background = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            ExchangeTabBorder.Background = new SolidColorBrush(Color.FromRgb(64, 64, 64));

            FirstCoinExchangeComboBox.Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            FirstCoinExchangeComboBox.Foreground = Brushes.White;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(FirstCoinExchangeComboBox, Brushes.White);
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetForeground(FirstCoinExchangeComboBox, new SolidColorBrush(Color.FromRgb(35, 35, 35)));
            MaterialDesignThemes.Wpf.ThemeAssist.SetTheme(FirstCoinExchangeComboBox, BaseTheme.Dark);

            ExchangeFirstCoinTextBox.BorderBrush = Brushes.LightGray;
            ExchangeFirstCoinTextBox.Foreground = Brushes.LightGray;
            ExchangeFirstCoinTextBox.Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            ExchangeFirstCoinTextBox.CaretBrush = Brushes.White;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(ExchangeFirstCoinTextBox, Brushes.LightGray);

            SwapButton.Background = Brushes.Gray;
            SwapButton.Foreground = new SolidColorBrush(Color.FromRgb(35, 35, 35));

            SecondCoinExchangeComboBox.Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            SecondCoinExchangeComboBox.Foreground = Brushes.White;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(SecondCoinExchangeComboBox, Brushes.White);
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetForeground(SecondCoinExchangeComboBox, new SolidColorBrush(Color.FromRgb(35, 35, 35)));
            MaterialDesignThemes.Wpf.ThemeAssist.SetTheme(SecondCoinExchangeComboBox, BaseTheme.Dark);

            ExchangeSecondCoinTextBox.BorderBrush = Brushes.LightGray;
            ExchangeSecondCoinTextBox.Foreground = Brushes.LightGray;
            ExchangeSecondCoinTextBox.Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            ExchangeSecondCoinTextBox.CaretBrush = Brushes.White;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(ExchangeSecondCoinTextBox, Brushes.LightGray);
        }
        private void SetLightColours()
        {
            DetailsTabExpanderFontColor = new SolidColorBrush(Color.FromRgb(32, 32, 32));
            DetailsTabExpanderBackColor = Brushes.WhiteSmoke;
            DetailsTabExpanderRankBorderColor = new SolidColorBrush(Color.FromRgb(179, 179, 179));
            DetailsTabExpanderBorderColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            DetailsTabExpDataBackColor = new SolidColorBrush(Color.FromRgb(237, 237, 237));
            DetailsTabExpDataBaseFontColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            DetailsTabExpDataSymbolFontColor = Brushes.Gray;
            DetailsTabExpDetailsColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            DetailsTabExpDetailsDataColor = Brushes.Black;
            DetailsTabExpDetailsBorderColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));

            DetailsTabExpMarketsBackColor = Brushes.White;
            DetailsTabExpMarketsTitleBackColor = new SolidColorBrush(Color.FromRgb(214, 214, 214));
            DetailsTabExpMarketsPairColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            DetailsTabExpMarketsBorderColor = Brushes.Gray;

            MainStackPanel.Background = Brushes.WhiteSmoke;
            CoinsInfoTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            MarketsInfoTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            CoinsInfoDataTextBlock.Foreground = Brushes.Red;
            MarketsInfoDataTextBlock.Foreground = Brushes.Red;
            LanguageThemeButton.Background = new SolidColorBrush(Color.FromRgb(32, 32, 32));
            LanguageThemeButton.Foreground = Brushes.White;

            MainCard.Background = Brushes.WhiteSmoke;
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetBackground(MainTabControl, Brushes.WhiteSmoke);
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetForeground(MainTabControl, Brushes.Black);
            TopTab.Foreground = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            countOfCoisCmbBox.Background = Brushes.WhiteSmoke;
            countOfCoisCmbBox.BorderBrush = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            MaterialDesignThemes.Wpf.ThemeAssist.SetTheme(countOfCoisCmbBox, BaseTheme.Light);
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(countOfCoisCmbBox, Brushes.Black);
            TitleGrid.Background = new SolidColorBrush(Color.FromRgb(214, 214, 214));

            DetailsTab.Background = Brushes.WhiteSmoke;
            SearchStackPanel.Background = new SolidColorBrush(Color.FromRgb(214, 214, 214));
            SearchTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            SearchTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            SearchTextBox.Background = Brushes.White;
            SearchTextBox.CaretBrush = Brushes.Black;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(SearchTextBox, new SolidColorBrush(Color.FromRgb(35, 35, 35)));


            TopTabSymbolColor = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            TopTabBorderRowColor = Brushes.Gray;

            ExchangeTab.Background = Brushes.WhiteSmoke;
            ExchangeTabBorder.Background = Brushes.LightGray;

            FirstCoinExchangeComboBox.Background = new SolidColorBrush(Color.FromRgb(176, 176, 176));
            FirstCoinExchangeComboBox.Foreground = Brushes.Black;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(FirstCoinExchangeComboBox, Brushes.Black);
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetForeground(FirstCoinExchangeComboBox, Brushes.Black);
            MaterialDesignThemes.Wpf.ThemeAssist.SetTheme(FirstCoinExchangeComboBox, BaseTheme.Light);

            ExchangeFirstCoinTextBox.BorderBrush = Brushes.Black;
            ExchangeFirstCoinTextBox.Foreground = Brushes.Black;
            ExchangeFirstCoinTextBox.Background = new SolidColorBrush(Color.FromRgb(176, 176, 176));
            ExchangeFirstCoinTextBox.CaretBrush = Brushes.Black;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(ExchangeFirstCoinTextBox, Brushes.Gray);

            SwapButton.Background = Brushes.Gray;
            SwapButton.Foreground = Brushes.White;

            SecondCoinExchangeComboBox.Background = new SolidColorBrush(Color.FromRgb(176, 176, 176));
            SecondCoinExchangeComboBox.Foreground = Brushes.Black;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(SecondCoinExchangeComboBox, Brushes.Black);
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetForeground(SecondCoinExchangeComboBox, Brushes.Black);
            MaterialDesignThemes.Wpf.ThemeAssist.SetTheme(SecondCoinExchangeComboBox, BaseTheme.Light);

            ExchangeSecondCoinTextBox.BorderBrush = Brushes.Black;
            ExchangeSecondCoinTextBox.Foreground = Brushes.Black;
            ExchangeSecondCoinTextBox.Background = new SolidColorBrush(Color.FromRgb(176, 176, 176));
            ExchangeSecondCoinTextBox.CaretBrush = Brushes.Black;
            MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(ExchangeSecondCoinTextBox, Brushes.Gray);
        }
        private void DarkOrLightThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (isLight)
            {
                isLight = false;

                DarkOrLightThemeButton.Content = FindResource("Dark");
                DarkOrLightThemeButton.Background = new SolidColorBrush(Color.FromRgb(32, 32, 32));

                SetLightColours();

                DetailedCoinsStackPanel.Children.Clear();
                TopCurrencyGrid.Children.Clear();
                TopCurrencyGrid.RowDefinitions.Clear();
                SetTopTabData(Convert.ToInt32(countOfCoisCmbBox.Text));
                SetDetailedCoinsTabData(DetailedCoinsTabData(), SearchTextBox.Text);
            }
            else
            {
                isLight = true;

                DarkOrLightThemeButton.Content = FindResource("Light");
                DarkOrLightThemeButton.Background = Brushes.LightGray;

                SetDarkColours();

                DetailedCoinsStackPanel.Children.Clear();
                TopCurrencyGrid.Children.Clear();
                TopCurrencyGrid.RowDefinitions.Clear();
                SetTopTabData(Convert.ToInt32(countOfCoisCmbBox.Text));
                SetDetailedCoinsTabData(DetailedCoinsTabData(), SearchTextBox.Text);
            }
        }

        //Method for EN/UA languages
        //I don't know how to do this another way
        bool isEnglish = true;
        private void LanguageThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (isEnglish)
            {
                isEnglish = false;
                LanguageThemeButton.Content = "UA";

                CoinsInfoTextBlock.Text = "Монети:";
                MarketsInfoTextBlock.Text = "Біржі:";
                TopTab.Header = "Топ монет";
                DetailsTab.Header = "Детальніше про монети";
                TopTabShowTextBlock.Text = "Показати топ";
                TopTabCoinsTextBlock.Text = "монет";
                TopTabTitleCoinTextBlock.Text = "Монета";
                TopTabTitlePriceTextBlock.Text = "Ціна";

                SearchTextBlock.Text = "Пошук:";
                MaterialDesignThemes.Wpf.HintAssist.SetHint(SearchTextBox, "Введіть назву монету або символ");
                SearchTextBox.Width = 250;

                ExchangeTab.Header = "Обмін";
                MaterialDesignThemes.Wpf.HintAssist.SetHint(FirstCoinExchangeComboBox, "Пошук");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(SecondCoinExchangeComboBox, "Пошук");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(ExchangeFirstCoinTextBox, "Введіть кількість монет");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(ExchangeSecondCoinTextBox, "Введіть кількість монет");
            }
            else
            {
                isEnglish = true;
                LanguageThemeButton.Content = "EN";

                CoinsInfoTextBlock.Text = "Coins:";
                MarketsInfoTextBlock.Text = "Markets:";
                TopTab.Header = "Top Coins";
                DetailsTab.Header = "Detailed Coins";
                TopTabShowTextBlock.Text = "Show top";
                TopTabCoinsTextBlock.Text = "coins";
                TopTabTitleCoinTextBlock.Text = "Coin";
                TopTabTitlePriceTextBlock.Text = "Price";

                SearchTextBlock.Text = "Search:";
                MaterialDesignThemes.Wpf.HintAssist.SetHint(SearchTextBox, "Enter coin name or symbol");
                SearchTextBox.Width = 200;

                ExchangeTab.Header = "Exchange";
                MaterialDesignThemes.Wpf.HintAssist.SetHint(FirstCoinExchangeComboBox, "Search");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(SecondCoinExchangeComboBox, "Search");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(ExchangeFirstCoinTextBox, "Enter coin amount");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(ExchangeSecondCoinTextBox, "Enter coin amount");
            }
        }

        //Method to add data on third tab (exchange)
        //Just add tabItems with coin name and symbol
        private void SetExchangeTabData()
        {
            CryptoResponse cryptoResponse = WebRequestData();

            bool selectedItem = true;
            foreach (var crypto in cryptoResponse.data)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();

                comboBoxItem.Content = $"{crypto.name} ({crypto.symbol})";
                comboBoxItem.Tag = crypto.id;

                ComboBoxItem copy = new ComboBoxItem();
                copy.Content = $"{crypto.name} ({crypto.symbol})";
                copy.Tag = crypto.id;

                FirstCoinExchangeComboBox.Items.Add(comboBoxItem);
                SecondCoinExchangeComboBox.Items.Add(copy);

                if (selectedItem)
                {
                    selectedItem = false;
                    FirstCoinExchangeComboBox.SelectedIndex = 0;
                    SecondCoinExchangeComboBox.SelectedIndex = 0;
                }
            }
        }

        //variables for TextBoxes validation
        Regex validationSymbolsFirstTextBox = new Regex("[^0-9.]+");
        Regex validationSymbolsSecondTextBox = new Regex("[^0-9.]+");

        //methods for TextBoxes validation
        private void NumberValidationTextBox1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = validationSymbolsFirstTextBox.IsMatch(e.Text);

        }
        private void NumberValidationTextBox2(object sender, TextCompositionEventArgs e)
        {
            e.Handled = validationSymbolsSecondTextBox.IsMatch(e.Text);

        }

        //variables which using for exchanges methods. 
        //Text from textBoxes updates after any changes occur TextChanged event calls the method in a loop
        //In this strings we note previous data and compare with the current
        //If they are equal, no change will occur
        string ChangedFirstTextBox = " ";
        string ChangedSecondTextBox = " ";

        //Next two methods for calculating count of needed coins
        private void ExchangeFirstCoinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox data = (TextBox)sender;
            if (data.Text.Contains(".")) validationSymbolsFirstTextBox = new Regex("[^0-9]+");
            else validationSymbolsFirstTextBox = new Regex("[^0-9.]+");

            if (ChangedFirstTextBox != ExchangeFirstCoinTextBox.Text && FirstCoinExchangeComboBox.SelectedIndex != -1 && SecondCoinExchangeComboBox.SelectedIndex != -1)
            {
                CryptoResponse cryptoResponse = WebRequestData();

                double firstCoinPrice = 1, secondCoinPrice = 1;
                foreach (var crypto in cryptoResponse.data)
                {
                    ComboBoxItem FirstCoin = (ComboBoxItem)FirstCoinExchangeComboBox.SelectedItem;
                    ComboBoxItem SecondCoin = (ComboBoxItem)SecondCoinExchangeComboBox.SelectedItem;
                    if (FirstCoin.Tag.ToString() == crypto.id) firstCoinPrice = Convert.ToDouble(crypto.priceUsd);
                    if (SecondCoin.Tag.ToString() == crypto.id) secondCoinPrice = Convert.ToDouble(crypto.priceUsd);
                }
                if (ExchangeFirstCoinTextBox.Text == "" || ExchangeFirstCoinTextBox.Text == ".") ExchangeSecondCoinTextBox.Text = "";
                else
                {
                    ChangedSecondTextBox = $"{Convert.ToDouble(ExchangeFirstCoinTextBox.Text) * firstCoinPrice / secondCoinPrice}";
                    ExchangeSecondCoinTextBox.Text = $"{Convert.ToDouble(ExchangeFirstCoinTextBox.Text) * firstCoinPrice / secondCoinPrice}";
                }
            }
        }

        private void ExchangeSecondCoinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox data = (TextBox)sender;
            if (data.Text.Contains(".")) validationSymbolsSecondTextBox = new Regex("[^0-9]+");
            else validationSymbolsSecondTextBox = new Regex("[^0-9.]+");
            if (ChangedSecondTextBox != ExchangeSecondCoinTextBox.Text && FirstCoinExchangeComboBox.SelectedIndex != -1 && SecondCoinExchangeComboBox.SelectedIndex != -1)
            {
                CryptoResponse cryptoResponse = WebRequestData();

                double firstCoinPrice = 1, secondCoinPrice = 1;
                foreach (var crypto in cryptoResponse.data)
                {
                    ComboBoxItem FirstCoin = (ComboBoxItem)FirstCoinExchangeComboBox.SelectedItem;
                    ComboBoxItem SecondCoin = (ComboBoxItem)SecondCoinExchangeComboBox.SelectedItem;
                    if (FirstCoin.Tag.ToString() == crypto.id) firstCoinPrice = Convert.ToDouble(crypto.priceUsd);
                    if (SecondCoin.Tag.ToString() == crypto.id) secondCoinPrice = Convert.ToDouble(crypto.priceUsd);
                }
                if (ExchangeSecondCoinTextBox.Text == "" || ExchangeSecondCoinTextBox.Text == ".") ExchangeFirstCoinTextBox.Text = "";
                else
                {
                    ChangedFirstTextBox = $"{Convert.ToDouble(ExchangeSecondCoinTextBox.Text) * secondCoinPrice / firstCoinPrice}";
                    ExchangeFirstCoinTextBox.Text = $"{Convert.ToDouble(ExchangeSecondCoinTextBox.Text) * secondCoinPrice / firstCoinPrice}";
                }
            }
        }

        //method for changing coin and update calculation
        private void CoinExchangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FirstCoinExchangeComboBox.SelectedIndex != -1 && SecondCoinExchangeComboBox.SelectedIndex != -1)
            {
                CryptoResponse cryptoResponse = WebRequestData();

                double firstCoinPrice = 1, secondCoinPrice = 1;
                foreach (var crypto in cryptoResponse.data)
                {
                    ComboBoxItem FirstCoin = (ComboBoxItem)FirstCoinExchangeComboBox.SelectedItem;
                    ComboBoxItem SecondCoin = (ComboBoxItem)SecondCoinExchangeComboBox.SelectedItem;
                    if (FirstCoin.Tag.ToString() == crypto.id) firstCoinPrice = Convert.ToDouble(crypto.priceUsd);
                    if (SecondCoin.Tag.ToString() == crypto.id) secondCoinPrice = Convert.ToDouble(crypto.priceUsd);
                }
                if (ExchangeFirstCoinTextBox.Text == "" || ExchangeFirstCoinTextBox.Text == ".") ExchangeSecondCoinTextBox.Text = "";
                else
                {
                    ChangedSecondTextBox = $"{Convert.ToDouble(ExchangeFirstCoinTextBox.Text) * firstCoinPrice / secondCoinPrice}";
                    ExchangeSecondCoinTextBox.Text = $"{Convert.ToDouble(ExchangeFirstCoinTextBox.Text) * firstCoinPrice / secondCoinPrice}";
                }
            }
        }

        //method for swap current coins (first = second, second = first)
        private void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            var temp = FirstCoinExchangeComboBox.SelectedIndex;
            FirstCoinExchangeComboBox.SelectedIndex = SecondCoinExchangeComboBox.SelectedIndex;
            SecondCoinExchangeComboBox.SelectedIndex = temp;
        }

        //method for change price format
        private decimal Price(string price)
        {
            decimal priceUsd = Convert.ToDecimal(price);
            decimal copyPrice = Convert.ToDecimal(price);
            int roundNums = 2;
            while(copyPrice < 1)
            {
                copyPrice *= 10;
                roundNums++;
            }
            return Math.Round(priceUsd,roundNums);
        }

    }
}
