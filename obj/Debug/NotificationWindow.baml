   M S B A M L   `   `   `  ����D  @LoginForm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:LoginForm	LoginForm  5   P   "    LoginForm.NotificationWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:LoginForm     6P   -   5          �Name$  MyWindow��5   	    � Title$ NotificationWindow��5   	   $	��370��6$   $	��283��61    � WindowStyle$
 None=�5   	    � AllowsTransparency . 
������  � 
ResizeMode$ 	CanResize=�5   	    J� SnapsToDevicePixels . &   System.Windows.Media.TextOptions   TextFormattingMode$ Display=�5   $    � 	Resources 5       g�Name   FadeOutStoryboard	 �� 
TargetName
 �� TargetProperty W� From	 W� To g� Duration g� FillBehavior)  #LoginForm.UrlToImageSourceConverter  UrlToImageSourceConverter  myHeaderStyle �� 
TargetType �� Property �� Value��%?  &	        &	 �     &	 �     �� 5   
   $ FadeOutStoryboard��6   -   5        .��5      W� $	 CentralLayout��6   $
 Opacity�6D   $ 1X�6h   $ 0X�6q   $ 0:0:0.7D�6x   $ HoldEnd=�6�   5        5   
   6N   �� 5   
   " ��6&   .�5      ��  � y�$ 	Collapsed=�6+   5      5      .��5      � -   5         �Name$ CentralLayout��5   7   $��14q�6+   $	��249��6N   .+�5   
   �� ���#8c8c8c$��1q�5       �� CornerRadius$ 2��5       J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ black��6#      ShadowDepth$ 0Q�61      	Direction$ 1Q�5   #      
BlurRadius$ 5Q�61      RenderingBias$ Quality=�5    #   5"      � 5$   
   
������� � RowDefinitions 5%      �� 5&      $
��auto �6$   64   �� 5'      6&   �� 5(      $
��auto �6$   64   .+�5+      �� -   5         ��Name$ 
ButtonExit��5+      $��0��5,      $��1355051979_Coffecup.png��5-      $
��Left=�5.      $��Uniform=�5/      $��
14,-24,0,7q�50      $��34��51      $��34��52      54      �� 55      -   5         ��Name$ Title��55      $��Need Developer��6+   $��18(�6A   $��	25 15 0 0q�6P   ���#6c6c6c �� TextAlignment$
 Left=�6x   $��0��6�   6�   � 56      $��1��6   $��0��6$   $��25 0 0 0q�6/   .+�57      �  58      �� 5:      $��* �6(   65   �� 5;      $��30 �6(   66   �� 5<      $��* �6(   65    � ColumnDefinitions 5>      �� 5?      $��* �6/   6;   �� 5@      $��35 �6/   6<   .+�5N      �� -   5        $��ButtonClickRight��5N      $��1��65   $��1��6B   $��
10,0,-10,0q�5Q      $
��Left=�60   $��35��5R      $��1.png��5S       5U        MouseOverAnimation  MouseOutAnimation��%m  &	       &	 �     �� 5V      $ MouseOverAnimation��6)   $	 ButtonClickRight��5W   )   $
 Opacity�5X   )   .��5Y   "   W� -   5        $	 ButtonClickRight��5Y   2   $ 0X�6[   $ 0:0:0.2D�6b   6u   5Z      �� 5[      $
 Color�6C   $
 Stop=�5\      .��5]   "   W� $	 ButtonClickRight��62   $ 1X�6[   $ 0:0:0.2D�6b   5^      5_      5`      5c      5e      �� 5f      $��2��6   $��1q�6'   ��5g      �� 5h       �� ImageSource$ 12.png��6%     j� Stretch$  UniformToFill=�6:   ._�5j      �  5k      �� 5l      $��* �6/   6;   �� 5m      $��55 �6/   6<   �� 5n      $��55 �6/   6<   .+�5p      �� $��Center=�6    $��0��5q       $��14(�60   
�������.��6X   	Job type:6d   �� 5r      -   5        ! ��Name$
! Type��5s   W   $��Center=�5r       $��1��5s       $��14(�60   
�������.��6e   Fixed6l   �� 5t      -	   5        $! Amount��5u   X   $��Center=�5t       $��2��5u       $��14(�61   
�������.��6h   Price6o   5v      5w      �� 5x      -
   5        $ Description��5x      $
��Left=�6-   $��	25,10,0,0q�6H   $��1��6[   $
��Wrap=�6h   $��	TextBlock��6|   $	��Top=�6�   $	��251��6�   $	��191��6�   5y      5{      " � Triggers" 5|      :� 5}   
   # :� RoutedEvent$# Window.Loaded��6   [�5~      �� 5      .��5�      �� .��5�      W� $ 0X�6*   $ 1X�63   $ 0:0:0.7D�6:   $	 CentralLayout��5�   +   $
 Opacity�5�   +   5�      5�      5�      5�      