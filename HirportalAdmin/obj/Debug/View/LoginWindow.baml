   M S B A M L   `   `   `  ����  HirportalAdmin%    HirportalAdmin.View.LoginWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � Title$  Bejelentkezés��5      $��153.249��6   $��221.414��6.    J� InputBindings 5      �� 5   
   
 �� Key$ Enter��6    �� Command 6!   �� *LoginCommand+.��5      �  � RowDefinitions 5	   
   �� 5
      $��35 �6   �� 5      $��35 �6   �� 5      $��35 �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��5q�6   $��0��6   $��0��6,   $��Felhasználónév: ��6<   �� 5   
   $��5q�6   $��0��6   $��1��6*   ��6:   �� *
UserName+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��5q�6   $��1��6   $��0��6,   $��	Jelszó: ��6<   G� 5   
   -   5        $��passwordBox��5      $��5q�6)   $��1��64   $��1��6A   �� 5   
   $��2��6   $��0��6"   $��2��62    �� Orientation$ 
Horizontal=�6F   $��Right=�6_   .+�5      �� $��Bejelentkezés��6   $��5,5,5,5q�6�   $	��108��6�   ��6-   �� *LoginCommand+��6N   ��  �� ElementName$ passwordBox��6�   �� 5      $��Mégse��6   $��5q�6E   $��81��6P   ��6%   �� *ExitCommand+6P   5      5      5      