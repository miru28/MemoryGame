   M S B A M L   `   `   `  ����E  AMemoryGAME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MemoryGAME.ViewsMemoryGAME.Views  5   	   '    !MemoryGAME.Views.CustomGameDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:MemoryGAME.Views     5   	     � Title$  Custom Game Settings��5   	   $	��200��6&   $	��300��63    � WindowStartupLocation$ CenterOwner=�5	   	    � 
ResizeMode$ NoResize=�6-   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6!   $��Rows:��61   $��0,0,10,5q�6>   �� 5   
   -   5         ��Name$ RowsComboBox��5   0   $��0��6   $��1��6    $��0,0,0,5q�6F   .@�5      �� $��2��6   �� 5      $��4��6    �� 
IsSelected$
 True��6'   �� 5      $��6��6   5      �� 5   
   $��1��6   $��0��6!   $��Columns:��61   $��0,0,10,5q�6A   �� 5   
   -   5        $ ColumnsComboBox��5   0   $��1��6   $��1��6    $��0,0,0,5q�6I   .@�5       �� $��2��6   �� 5!      $��4��6   $
 True��6'   �� 5"      $��6��6   5#      �� 5%   
   $��2��6   $��0��6!   $��2��61   $(��"Total cards must be an even number��5&      
�������$	��0,5q�6P   �� 5(   
   $��4��6   $��0��6"   $��2��62    �� Orientation$ 
Horizontal=�5)      $��Right=�6.   .+�5*      �� -   5        $��OK��5*      $��80��6"   $��0,0,10,0q�6-   6?   �� 5+      -   5        $��Cancel��5+      $��80��6&   61   5,      5-      5.      