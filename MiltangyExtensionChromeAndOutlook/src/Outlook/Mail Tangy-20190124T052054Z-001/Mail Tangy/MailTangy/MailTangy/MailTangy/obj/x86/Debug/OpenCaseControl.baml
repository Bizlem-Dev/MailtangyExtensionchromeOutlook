   M S B A M L   `   `   `  ����D  @MailTangy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MailTangy	MailTangy  5          MailTangy.OpenCaseControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:MailTangy     5        � 	Resources  5	      &    MailTangy.EnableDisableConverter   
enableNext)   #MailTangy.EnableDisablePreConverter  enablePrevious �� 
TargetType  	NavButton �� Property �� Value  OpenCaseTemplate �� Path  ClosedCaseTemplate  AutpRepliedCaseTemplate��%�  &	        &	      &	 &     &	 �    &	 �    &	 �      5
   
   6'     5   
   6*   �� 5   
   " ��6   .�5      ��  � ��$ 15��6&   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 15��6'   �� 5       � ��$ 10(�6)   �� 5       � ��$ 3q�6'   �� 5       � ��$ 0q�60   �� 5       � ��$ 10(�6)   �� 5       � ��$ Segoe MDL2 Assets)�6+   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   �� 5       � ��
 �����6+   5      �� 5   
   .R�5      �� .+�5      �� 
����  �$
��Wrap=�5      ��5      �� $ CaseSubject�5      �� 5       $
��Bold"�5!      ��5       �� $ DateTimeOpened�5!      5#      5$      �� 5%   
   .R�5&      �� .+�5'      �� 
����  �$
��Wrap=�5)      ��5'      �� $ CaseSubject�5)      �� 5+      $
��Bold"�5,      ��5+      �� $ DateTimeClosed�5,      5.      5/      �� 50   
   .R�51      �� .+�52      �� $
��Wrap=�53      
����  ���52      �� $ Subject�54      �� 56      $��SemiBold"�57      $��10(�58      ��56      �� $ ReceivedDate�58      5:      5;      5<      .��5=      �  � RowDefinitions 5>   
   �� 5?      $
��Auto �6   �� 5@      $
��Auto �6   �� 5A      $
��Auto �6   �� 5B      $
��Auto �6   .+�5E   
   9� -   5         9�Name$ OpenCasesExpander��5F       9� 
IsExpanded .  ���	#FF3074AE
�������$��10,5,0,0q�5J      ��5I      �� *OpenCasesHeader+.��5K      �� 
���  ��
�������$��0q�5M      ._�5O      �� .+�5P      �� $��Stretch=�6   $	��270��5Q      $��0q�5R      $��Disabled=�5T      $��Disabled=�5U      #��[ 5V      ��5S      �� *OpenCasesSubset+5V      �� 5W       �� Orientation$ 
Horizontal=�6   
�������$��Center=�5Y   !   .+�5Z      �� -   5        	 ��Name$	 OPreviousPage��5Z   !   #��[ 5\      $	����5]      {�5^      `� 5_   "   
 `� 	Converter#
 [ 6/   .1�5`   &   ��  �� ElementName$ OPageNo��6.   $ Content�6D   �� 5a   &   $ OpenCasesExpander��6.   $ Header�6N   5b   #   5d      �� 5e      -   5         ��Name$ OPageNo��5i      $��Center=�5f      
���   �$��40��5h      $��3q�5j      ��5e       �� *OPageNumber+5j      �� 5k      -   5        $	 	ONextPage��5k   !   #��[ 5m      $	����5n      {�5o      `� 5p   "   #
 [  6/   .1�5q   &   �� $ OPageNo��6.   $ Content�6D   �� 5r   &   $ OpenCasesExpander��6.   $ Header�6N   5s   #   5u      5w      5x      5y      5z      9� 5|   
   -   5        $ ClosedCasesExpander��5|       $��1��6    .  ���	#FF3074AE
�������$��Disabled=�5�      $��10,5,0,0q�5�      ��5}      �� *ClosedCasesHeader+.��5�      �� 
��� � �$��0q�6)   
�������._�5�      �� .+�5�      �� $��Stretch=�6   $	��270��5�      $��0q�5�      $��Disabled=�5�      $��Disabled=�5�      #��[ 5�      ��5�      �� *ClosedCasesSubset+5�      �� 5�      $ 
Horizontal=�6!   
�������$��Center=�5�   !   .+�5�      �� -   5        $	 CPreviousPage��5�   !   #��[ 5�   !   $	����5�      {�5�      `� 5�   "   #
 [ 6/   .1�5�   &   �� $ CPageNo��6.   $ Content�6D   �� 5�   &   $ ClosedCasesExpander��6.   $ Header�6P   5�   #   5�      �� 5�      -   5        $ CPageNo��5�       $��40��5�       $��Center=�5�       
���   �$��3q�5�       ��5�       �� *CPageNumber+5�       �� 5�      -   5        $	 	CNextPage��5�   !   #��[ 5�   !   $	����5�      {�5�      `� 5�   "   #
 [  6/   .1�5�   &   �� $ CPageNo��6.   $ Content�6D   �� 5�   &   $ ClosedCasesExpander��6.   $ Header�6P   5�   #   5�      5�      5�      5�      5�      9� 5�   
   -	   5        $ AutoRepliedCasesExpander��5�       $��2��6    .  ���	#FF3074AE$��Disabled=�5�      
�������$��10,5,0,0q�5�      ��5�      �� *AutoReplyCasesHeader+.��5�      �� 
��� ���
�������$��0q�5�      ._�5�      �� .+�5�      �� $��Stretch=�6   $��0q�5�      $	��270��5�      #��[ 5�      ��5�      �� *AutoRepliedCasesSubset+5�      �� 5�      $ 
Horizontal=�6!   
�������$��Center=�5�   !   .+�5�      �� -
   5        $	 APreviousPage��5�   !   #��[ 5�   !   $	����5�   !   {�5�      `� 5�   "   #
 [ 6/   .1�5�   &   �� $ APageNo��6.   $ Content�6D   �� 5�   &   $ AutoRepliedCasesExpander��6.   $ Header�6U   5�   #   5�      �� 5�      -   5        $ APageNo��5�       $��40��5�       $��Center=�5�       
���   �$��3q�5�       ��5�       �� *APageNumber+5�       �� 5�      -   5        $	 	ANextPage��5�   !   #��[ 5�   !   $	����5�   !   {�5�      `� 5�   "   #
 [  6/   .1�5�   &   �� $ APageNo��6.   $ Content�6D   �� 5�   &   $ AutoRepliedCasesExpander��6.   $ Header�6U   5�   #   5�      5�      5�      5�      5�      9� 5�   
   -   5        $ custSentimentIndex��5�      $��3��5�      ���	#FF3074AE . $��10,5,0,0q�5�      
�������$��Customer Sentiment Index��5�      .��5�      �� 
��� ���
�������$��1q�5�      ._�5�      �  � ColumnDefinitions 5�      �� 5�      $��* �6+   �� 5�      $��* �6+   �� 5�      $��* �6+   .+�5�      �� .+�5�      �� $��Resources/smile_happy.png��6    $��50��5�       $��10q�5       �� 5     $��Center=�5  $   
���   ���5  $   �� *
HappyCount+5  $   5     �� 5     $��2��6!   .+�5     �� $��Resources/smile_sad.png��6    $��50��5      $��10q�5      �� 5	     
���   �$��Center=�5  $   ��5	  $   �� *
SadCount+5  $   5     �� 5     $��1��6!   .+�5     �� $"��Resources/smile_confused.png��6    $��50��5      $��10q�5      �� 5     
���   �$��Center=�5  $   ��5  $   �� *ConfusedCount+5  $   5     5     5     5     5     5     