   M S B A M L   `   `   `  ����A  =klient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:klientklient  5   	       klient.Lobby   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:klient     5   	     � Title$  Lobby��5   	   $	��450��6   $	��800��6$   .��5	      �  � ColumnDefinitions 5
   
   �� 5      6!   �� 5      6!   �� 5      6!    � RowDefinitions 5   
   �� 5      6   �� 5      $��5* �6   6*   .+�5   
   �� $��1��6   $��30(�6$   $��0,0,0,0q�62   $
��Wrap=�6C   $��Poczekalnia��6W   $��Center=�6j   $��Center=�6�   �� 5   
   -   5         ��Name$ sendBtn��5      $
��Send��6"   $��0��61   $��1��6A   $
��Left=�6N   $��187,312,0,0q�6i   $	��Top=�6~   $��30��6�   $��78��6�   �� 5   
   -   5         ��Name$ sendTbox��5      $
��Left=�6$   $��	2,312,0,0q�6?    �� TextWrapping$
 Wrap=�6R   $�� ��6f   $	��Top=�6n   $	��177��6�   $��30��6�   $��1��6�   �� 5   
   $��1q�6   
���   �$��Center=�69   $��0,69,0,0q�6V   $	��Top=�6h   $	��263��6�   $	��300��6�   $��2��6�   ._�5      �� -   5         ��Name$ receivedTbox��5      $��0��6.   $��1��6>   $
��Left=�6K   $��4,0,0,0q�6f   $
��Wrap=�6w   $�� ��6�   $��Center=�6�   $	��258��6�   $	��290��6�   5      �� 5   
   -   5         ��Name$ serverLeaderBoard��5      $��1��6.   $��2��6;   $��0,0,0,0q�6K    �� View 5      �� 5      .P�5      �� $��Leaderboard��6%   	 �� Width$		 250��6:   6H   5      5      �� 5    
   -   5        $ searchGameButton��5       $��	Play Game��6+   $��25(�6?   $��DemiBold"�6M   $��1��6d   $	��150��6t   $��70��6�   $��Center=�6�   $��Center=�6�   $��1��6�   6�   5"      5#      