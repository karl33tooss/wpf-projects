   M S B A M L   `   `   `  ????D  @CryptoApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:CryptoApp	CryptoApp  5   	       CryptoApp.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:CryptoApp     5   	   _ [MaterialDesignThemes.Wpf, Version=4.7.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     ? Title$  
MainWindow??5	   	   $	??520??6   $	??600??6)    ? 
ResizeMode$ NoResize=?65    ? 	Resources 5
      '  !MaterialDesignThemes.Wpf.PackIcon 	  Light   Kind  Dark  TabControlLightBackground.  (MaterialDesignThemes.Wpf.ColorZoneAssist   
Background  TabControlLightForeground   
Foreground??%,  &	        &	 ]     &	 ?     &	 ?       5   
   $ WeatherSunny=?5   %   ???#232323$??15??5   %   $??15??5   %     5   
   $ MoonWaningCrescent=?5   %   
???????$??15??5   %   $??15??5   %   ?? 5   
    ?#2323235      ?? 5   
   
 ?????5      5      .??5      ?? -   5         ??Name$ MainStackPanel??5      ???#232323.+?5   
   ?? 
???????$??0 0 0 1q?5      ._?5      ]? $??30??5       .+?5!      ?? -   5         ??Name$ CoinsInfoTextBlock??5%      $??Coins:??5!      
???????$??Center=?5#      $??15 0 0 0q?5$      5%      ?? 5&      -   5        $ CoinsInfoDataTextBlock??5*      $	??100??5&      
???????$??Center=?5(      $??5 0 0 0q?5)      5*      ?? 5+      -   5        $ MarketsInfoTextBlock??5/      $??Markets:??5+      
???????$??Center=?5-      $??25 0 0 0q?5.      5/      ?? 50      -   5        $ MarketsInfoDataTextBlock??54      $??25??50      
???????$??Center=?52      $??5 0 0 0q?53      54      ?? 55       ?? Orientation$ 
Horizontal=?6   $??Right=?56      .+?57      ?? -   5        	 ??Name$	 DarkOrLightThemeButton??57        MaterialDesignIconButton#??[ 58      $??20??59      $??20??5:      $??15 0 0 0q?5;      
???????
???????
 ?????.??5@      C? 
 C? ResourceKey$
 Light??6*   5A      ?? 5B      -   5        $	 LanguageThemeButton??5B      #??[ 5C      $??20??5D      $??20??5E      $
??10 0q?5F      
???????
??????????#232323$??10(?5J      $
??Bold"?5K      $??EN??5L      5N      5O      5P      5Q      #  MaterialDesignThemes.Wpf.Card  5R   
   -   5          Name$ MainCard??5S      ???#232323.??5T      ?? -	   5         ??Name$ MainTabControl??5Y      $
??Left=?5T       " MaterialDesignFilledTabControl#??[ 5U         Mode$ Custom=?5V      
 ????? ?#232323.@?5Z      ?? -
   5         ??Name$ TopTab??5\      $??	Top Coins??5Z      
???????.??5]      ?? $	??350??6!   .+?5^      ?? $ 
Horizontal=?6%   .+?5_      ?? -   5        $ TopTabShowTextBlock??5d   (   $??Show top??5_   (   $??Center=?5`   (   $??15(?5a   (   $
??Bold"?5b   (   $??10q?5c   (   5d   (   ?? 5e      -   5         ??Name$ countOfCoisCmbBox??5i   '   $??15(?5f   '   $
??Bold"?5g   '   $	??5 0q?5h   '   ???#232323
???????*  $MaterialDesignThemes.Wpf.ThemeAssist   Theme$
 Dark=?5l   '   .  (MaterialDesignThemes.Wpf.TextFieldAssist   UnderlineBrush
 ?????.@?5o   "   ?? $??5??6/   ?? 5p   "   $??10??6/   ?? 5q   "   $??15??6/    ?? 
IsSelected . 5r   /   ?? 5s   "   $??20??6/   ?? 5t   "   $??25??6/   5u      ?? 5v      -   5        $ TopTabCoinsTextBlock??5{   (   $??coins??5v   (   $??Center=?5w   (   $??15(?5x   (   $
??Bold"?5y   (   $??10q?5z   (   5{   (   5|      ? 5}      -   5         ?Name$ 	TitleGrid??5      $??40??5}      ???#303030 ? ColumnDefinitions 5?      ?? 5?   "   $??1* ?63   ?? 5?   "   $??5* ?63   ?? 5?   "   $??7* ?63   ?? 5?   "   $??2* ?63   .+?5?      ?? $??#??6(   $??14(?5?   (   $
??Bold"?5?   (   $??Center=?5?   (   $??Center=?5?   (   $??0??5?   (   ?? 5?      -   5        $ TopTabTitleCoinTextBlock??5?   (   $
??Coin??5?   (   $??14(?5?   (   $
??Bold"?5?   (   $
??Left=?6`   $??Center=?5?   (   $??15 0 0 0q?5?   (   $??1??5?   (   5?   (   ?? 5?      -   5        $ TopTabTitlePriceTextBlock??5?   (   $??Price??5?   (   $??14(?5?   (   $
??Bold"?5?   (   $??Right=?5?   (   $??Center=?5?   (   $
??35 0q?5?   (   $??2??5?   (   5?   (   ?? 5?      $	??24h??6(   $??14(?5?   (   $
??Bold"?5?   (   $??Center=?5?   (   $??Center=?5?   (   $??0 0 20 0q?5?   (   $??3??5?   (   5?      ?? 5?      ??. $	??320??5?   '   .??5?      ? -   5        $ TopCurrencyGrid??5?   #    5?   "   ?? 5?   &   $??1* ?67   ?? 5?   &   $??1* ?67   ?? 5?   &   $??3* ?67   ?? 5?   &   $
??1.5* ?67   ?? 5?   &   $??7* ?67   ?? 5?   &   $??2* ?67   5?      5?      5?      5?      ?? 5?      -   5        $ 
DetailsTab??5?      $??Detailed Coins??5?      ???#232323.??5?      ?? .+?5?      ?? -   5        $ SearchStackPanel??5?   %   $ 
Horizontal=?5?   %   ???#303030$??50??5?   )   .+?5?      ?? -   5        $ SearchTextBlock??5?   (   $??Search:??5?   (   $??18(?5?   ,   $
??Bold"?5?   ,   
???????$??Center=?5?   ,   $??15 5 5 5q?5?   ,   5?   (   ?? 5?      -   5         ??Name$ SearchTextBox??5?   *   $??Center=?5?   &   $?? ??5?   *   $	??200??5?   *   $??30??5?   *   $??5q?5?   *   $??14(?5?   *   
???????
??????????#505050 ?? 
CaretBrush
 ?????$??5q?5?   *   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Enter coin name or symbol??5?   *   
 ?????   TextFieldCornerRadius$ 5??5?   *   5?   *   5?      ?? 5?      ??. $	??350??5?   '   .??5?      ?? -   5        $ DetailedCoinsStackPanel??5?   )   5?      5?      5?      ?? 5?      -   5        $ ExchangeTab??5?      $??Exchange??5?      ???#232323.??5?      ?? .+?5?      ?? -   5         ??Name$ ExchangeTabBorder??5?   !   ???#404040$??Center=?5?   !   $	??350??5?   !   $??10q?5?   !   $	??380??5?   !    ?? CornerRadius$ 20??5?   !   ._?5?      ?? .+?5?   "   ?? -   5        $ FirstCoinExchangeComboBox??5?   +   $??20q?5?   +   ???#505050$??20 10q?5?   +   $??15(?5?   +   $
??Bold"?5?   +   
???????$ 	10 10 0 0??5?   +   
 ????? ?#232323$
 Dark=?5?   +   $ Search??5?   +      HintOpacity$ .5Q?5?   +   -  'MaterialDesignThemes.Wpf.ComboBoxAssist   	MaxLength$ 20??5?   +    ?? 
IsEditable . 5?   #   ?? 5?   "   -   5        $ ExchangeFirstCoinTextBox??5  *   $??Center=?5?   *   $?? ??5?   *   $
??20 0q?5?   *   $??50??5?   *   $??16(?5?   *   
???????
??????????#505050
 ?????$??10q?5?   *   $ Enter coin amount??5   *   
 ?????$ 	0 0 10 10??5  *   5  *   ?? 5  "   -   5        $	 
SwapButton??5  )   #??[ 5  )    ? ToolTip$ MaterialDesignIconButton??5  )   
??????????#232323$??20q?5  )   .??5  &     $ SwapVertical=?5  1   5  #   ?? 5  "   -   5        $  SecondCoinExchangeComboBox??5  +   $??
20 0 20 20q?5  +   ???#505050
???????$??20 10q?5  +   $??15(?5  +   $
??Bold"?5  +   $ 	10 10 0 0??5  +   
 ????? ?#232323$
 Dark=?5  +   $ Search??5  +   $ .5Q?5  +   $ 20??5  +    . 5!  #   ?? 5"  "   -   5        $ ExchangeSecondCoinTextBox??50  *   $??Center=?5"  *   $?? ??5#  *   $
??20 0q?5$  *   $??50??5%  *   $??16(?5&  *   
???????
??????????#505050
 ?????$??10q?5+  *   $ Enter coin amount??5,  *   
 ?????   UnderlineCornerRadius$ 10??5.  *   $ 	0 0 10 10??5/  *   52  *   53     54     55     56     57     58     59     5:     