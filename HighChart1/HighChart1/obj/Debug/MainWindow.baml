   M S B A M L   `   `   `  ����E  AHighChart1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:HighChart1
HighChart1  5   	   'clr-namespace:HighChart1
HighChart1  5   	       HighChart1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0MyNamespaceclr-namespace:HighChart1     5   	   *localclr-namespace:HighChart1     5   	   -   5          � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��0.79* �6   �� 5      $�� 0.01* �6   �� 5      $
��0.2* �6   .+�5   
   � .+�5         HighChart1.CustomChart  -   5          Name$ mychart��5   &   5      � 5   
   -   5         �Name$ chart��5      $��1��6   .+�5      �� -   5        $��DisplayImage��5      5      � 5   
   $��2��6   .+�5      �� -   5         ��Name$ randomChart��5      $��随机直线��6*   $
��Left=�69   $��43��6T   $��	43,48,0,0q�6`   $	��Top=�6s   $��83��6�   6�   �� 5      -   5        $ realTimeChart��5      $��实时曲线��6,   $
��Left=�6;   $��42��6V   $��
43,130,0,0q�6b   $	��Top=�6v   $��83��6�   6�   �� 5      -   5        $ realTimeChartbyControl��5      $��控件内实时曲线��65   $
��Left=�6G   $��42��6b   $��
39,208,0,0q�6n   $	��Top=�6�   $	��101��6�   6�   5      5!      5"      