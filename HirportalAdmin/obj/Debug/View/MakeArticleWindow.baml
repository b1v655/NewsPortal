   M S B A M L   `   `   `  ����I  EHirportalAdmin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY1clr-namespace:HirportalAdmin.ViewModel.Converters#HirportalAdmin.ViewModel.Converters  5   	   +    %HirportalAdmin.View.MakeArticleWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   G	viewModel1clr-namespace:HirportalAdmin.ViewModel.Converters     5   	     � Title$&   Hírportál - Cikk szerkesztése��5   	   $	��340��6/   $	��600��6<    � 	Resources 5	      ?   9HirportalAdmin.ViewModel.Converters.ArticleImageConverter   articleImageConverter��%   &	          5
   
   6*   5      .��5      �  � RowDefinitions 5   
   �� 5      $��32 �6   �� 5      $��32 �6   �� 5      $��32 �6   �� 5      $��32 �6   �� 5      $	��100 �6   �� 5      $��32 �6   �� 5      $��35 �6    � ColumnDefinitions 5   
   �� 5      $	��400 �6   �� 5      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� $��Cím:��6   $��5q�6$   �� 5      $	��150��6   $��5q�6G   ��6"   �� *EditedArticle.Title+6G   5      �� 5   
   $��1��6   $ 
Horizontal=�6"   .+�5       �� $��Összegzés:��6   $��5q�6*   �� 5!      $	��200��6   $��5q�6I   ��6"   �� *EditedArticle.Summary+6I   5"      �� 5#   
   $��2��6   $ 
Horizontal=�6"   $��2��6;   .+�5$      �� $��	Tartalom:��6   $��5q�6)   �� 5%      $	��300��6   $��5q�6I   ��6"   �� *EditedArticle.Content+6I   5&      �� 5'   
   $��3��6   $ 
Horizontal=�6"   $��2��6;   .+�5(      �� $��	Főcikk: ��6   $��5q�6(   �� 5)      $��5q�6J    d� 	IsChecked 6   �� *EditedArticle.IsMainArticle+6J   5*      �� 5+   
   -   5        $��imageListBox��5+   3   $��4��6   $��2��6   $
��Auto=�6t   $��Disabled=�6�   ��6G   �� *EditedArticle.Images+��5-      �� 5.      .R�5/      �  K� IsItemsHost . 6    51      ��53      �� 54      .R�55      �� $	��100��6   $
��Auto��6)   ��66   �� *Image+ �� 	Converter# [  57      59      � 5;   
   $��5��6   $��2��6   .+�5<      �� $��Új kép hozzáadása��6   $��6,5,0,5q�61   $
��Left=�6�   $	��120��6�   ��6B   �� *CreateImageCommand+��6i   �� *EditedArticle+{�6�   �� *
IsLoaded+�� 5=      $��Kijelölt kép törlése��6   $��0,5,6,5q�64   $��Right=�6�   $	��120��6�   ��6E   �� *DeleteImageCommand+��6l   ��  �� ElementName$ imageListBox��	 �� Path$	 SelectedItem�{�6�   �� *
IsLoaded+5>      �� 5?   
   $��6��6   $ 
Horizontal=�6"   $��2��6;   $��Right=�6O   .+�5@      �� $��Mentés��6   $��6,5,0,5q�6&   $	��100��6^   ��67   �� *SaveChangesCommand+6^   �� 5A      $��Mégse��6   $��5,5,6,5q�6%   $	��100��6_   ��66   �� *CancelChangesCommand+6_   5B      5C      5D      