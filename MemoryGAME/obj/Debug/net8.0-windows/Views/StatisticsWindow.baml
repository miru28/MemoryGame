   M S B A M L   `   `   `  ����E  AMemoryGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MemoryGAME.ViewsMemoryGAME.Views  5   	   '    !MemoryGAME.Views.StatisticsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:MemoryGAME.Views     5   	     � Title$  Game Statistics��5   	   $	��400��6!   $	��500��6.    � WindowStartupLocation$ CenterOwner=�5	   	   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Player Statistics��6!   $��20(�6:   $��0,0,0,10q�6H   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ StatisticsGrid��5       $��1��6      AutoGenerateColumns$ False��68      
IsReadOnly$
 True��5         GridLinesVisibility$	 All=�6%      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Username��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 * 6T   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 67   �� *
Username+6T     5      $ Games Played��6%   $	 * 6[   
 6;   �� *GamesPlayed+6[     5      $ 	Games Won��6%   $	 * 6U   
 68   �� *
GamesWon+6U   5      �� 5   
   -   5        $��2��5      $��Close��6   $��Right=�6.   $��0,10,0,0q�5      $
��20,5q�6#   62   5      5      