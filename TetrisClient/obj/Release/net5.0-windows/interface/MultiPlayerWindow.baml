   M S B A M L   `   `   `  ????   TetrisClient.    (TetrisClient.interface.MultiPlayerWindow   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ???#A3D30E  ? 
ResizeMode$  NoResize=? ? Title$ Tetris - Multi Player??$	??450??$
??1000??.??? $??0,0,0,-6q?.+??? -    ??Name$ P1??$??24(????#375A10$??1??$
??Left=?$??
550,25,0,0q?$
??Wrap=?$	??Top=???  ? Language$ en-gb?$?P1??? -    ?Name$ TetrisGridP1??
???   ?$??1??$	??400??$
??Left=?$??540,25,0,25q?$	??250?? ? RowDefinitions ?? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ? ? ColumnDefinitions ?? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ?? -   $ 
NextGridP1??
???   ?$??1??$	??100??$
??Left=?$??
840,25,0,0q?$	??Top=?$	??100?? J? RenderTransformOrigin$ 
-0.3,0.367&? ?? $??25 ??? $??25 ??? $??25 ??? $??25 ? ?? $??25 ??? $??25 ??? $??25 ??? $??25 ??? -   $ PointsP1??$??24(????#375A10$??1??$
??Left=?$??800,305,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?Score???? -   $ LinesP1??$??24(????#375A10$??1??$??35??$
??Left=?$??800,254,0,0q?$
??Wrap=?$	??Top=??? $?L???? $
?ines???? ?? ?? -   $ LevelP1??$??24(????#375A10$??1??$??35??$
??Left=?$??800,201,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?L???? $
?evel???? ?? ?? -   $ LevelTextBlockP1??$??24(????#375A10$??1??$??35??$
??Left=?$??870,203,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?0???? -   $ LinesTextBlockP1??$??24(????#375A10$??1??$??35??$
??Left=?$??870,256,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?0???? -	   $ PointsTextBlockP1??$??24(????#375A10$??1??$??35??$
??Left=?$??870,306,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?0???? -
   $ GameOverTextP1??$??30(????#A3D30E$??1??$	??257??$
??Left=?$??	515,0,0,0q? ?? TextAlignment$ Center=?$
??Wrap=?$??Center=?$y?Hidden=?$	??298???? $?	GAME OVER???? ?? $
?PRES???? $?S ENTER???? ?? $?
TO RESTART???? -   $ P1Text??$??24(????#375A10$??1??$
??Left=?$??
545,25,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?P1???? -   	 ??Name$	 	StartGame?????#375A10???#A3D30E$??Start??$|?False??$??30??$
??Left=?$??475,210,0,0q?$	??Top=?$??50????Z? ?? -   $	 
QuitButton?????#375A10???#A3D30E$
??Quit??$|?False??$??30??$
??Left=?$??475,260,0,0q?$	??Top=?$??50????Z? ? -   $ TetrisGridP2??
???   ?$??1??$	??400??$
??Left=?$??210,25,0,25q?$	??250?? ?? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ? ?? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ??? $??25 ?? -   $ 
NextGridP2??
???   ?$??1??$	??100??$
??Left=?$??	60,25,0,0q?$	??Top=?$	??100??$ 
-0.3,0.367&? ?? $??25 ??? $??25 ??? $??25 ??? $??25 ? ?? $??25 ??? $??25 ??? $??25 ??? $??25 ??? -   $ PointsP2??$??24(????#375A10$??1??$
??Left=?$??
25,305,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?Score???? -   $ LinesP2??$??24(????#375A10$??1??$??35??$
??Left=?$??
25,254,0,0q?$
??Wrap=?$	??Top=??? $?L???? $
?ines???? ?? ?? -   $ LevelP2??$??24(????#375A10$??1??$??35??$
??Left=?$??
25,201,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?L???? $
?evel???? ?? ?? -   $ LevelTextBlockP2??$??24(????#375A10$??1??$??35??$
??Left=?$??100,203,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?0???? -   $ LinesTextBlockP2??$??24(????#375A10$??1??$??35??$
??Left=?$??100,256,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?0???? -   $ PointsTextBlockP2??$??24(????#375A10$??1??$??35??$
??Left=?$??100,306,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?0???? -   $ GameOverTextP2??$??30(????#A3D30E$??1??$	??257??$
??Left=?$??	185,0,0,0q?$ Center=?$
??Wrap=?$??Center=?$y?Hidden=?$	??298???? $?	GAME OVER???? ?? $
?PRES???? $?S ENTER???? ?? $?
TO RESTART???? -   $ P2Text??$??24(????#375A10$??1??$
??Left=?$??
215,25,0,0q?$
??Wrap=?$	??Top=??? $ en-gb?$?P2??