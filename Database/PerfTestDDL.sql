S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 S E T   A N S I _ P A D D I N G   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ P E R F _ T e s t S u i t e R e s u l t ] (  
 	 [ T e s t S u i t e R e s u l t I D ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ U s e r I D ]   [ u n i q u e i d e n t i f i e r ]   N O T   N U L L ,  
 	 [ S t a r t T i m e ]   [ d a t e t i m e ]   N O T   N U L L ,  
 	 [ E n d T i m e ]   [ d a t e t i m e ]   N O T   N U L L ,  
 	 [ E l a p s e d T i m e M S e c s ]   [ i n t ]   N O T   N U L L ,  
 	 [ A n n o t a t i o n ]   [ v a r c h a r ] ( 5 0 0 )   N O T   N U L L ,  
   C O N S T R A I N T   [ P K _ P E R F _ T e s t R u n ]   P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ T e s t S u i t e R e s u l t I D ]   A S C  
 ) W I T H   ( P A D _ I N D E X     =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E     =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S     =   O N ,   A L L O W _ P A G E _ L O C K S     =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
 G O  
 S E T   A N S I _ P A D D I N G   O F F  
 G O  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 S E T   A N S I _ P A D D I N G   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ P E R F _ T e s t R e s u l t ] (  
 	 [ T e s t R e s u l t I D ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ T e s t S u i t e R e s u l t I D ]   [ i n t ]   N O T   N U L L ,  
 	 [ C l a s s N a m e ]   [ v a r c h a r ] ( 2 0 0 )   N O T   N U L L ,  
 	 [ M e t h o d N a m e ]   [ v a r c h a r ] ( 2 0 0 )   N O T   N U L L ,  
 	 [ S t a r t T i m e ]   [ d a t e t i m e ]   N O T   N U L L ,  
 	 [ E n d T i m e ]   [ d a t e t i m e ]   N O T   N U L L ,  
 	 [ E l a p s e d T i m e M S e c s ]   [ i n t ]   N O T   N U L L ,  
 	 [ I s S u c c e s s ]   [ b i t ]   N O T   N U L L ,  
 	 [ A n n o t a t i o n ]   [ v a r c h a r ] ( 5 0 0 )   N O T   N U L L ,  
   C O N S T R A I N T   [ P K _ P E R F _ T E S T D A T A ]   P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ T e s t R e s u l t I D ]   A S C  
 ) W I T H   ( P A D _ I N D E X     =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E     =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S     =   O N ,   A L L O W _ P A G E _ L O C K S     =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
 G O  
 S E T   A N S I _ P A D D I N G   O F F  
 G O  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 S E T   A N S I _ P A D D I N G   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ P E R F _ S y s t e m I n f o ] (  
 	 [ S y s t e m I n f o I D ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ T e s t S u i t e R e s u l t I D ]   [ i n t ]   N O T   N U L L ,  
 	 [ C o m p o n e n t ]   [ v a r c h a r ] ( 5 0 )   N O T   N U L L ,  
 	 [ N a m e ]   [ v a r c h a r ] ( 1 0 0 )   N O T   N U L L ,  
 	 [ V a l u e ]   [ v a r c h a r ] ( 2 0 0 )   N O T   N U L L ,  
   C O N S T R A I N T   [ P K _ P E R F _ S y s t e m I n f o ]   P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ S y s t e m I n f o I D ]   A S C  
 ) W I T H   ( P A D _ I N D E X     =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E     =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S     =   O N ,   A L L O W _ P A G E _ L O C K S     =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
 G O  
 S E T   A N S I _ P A D D I N G   O F F  
 G O  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 S E T   A N S I _ P A D D I N G   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ P E R F _ T i m i n g D a t a ] (  
 	 [ T i m i n g D a t a I D ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ T e s t R e s u l t I D ]   [ i n t ]   N O T   N U L L ,  
 	 [ C l a s s N a m e ]   [ v a r c h a r ] ( 2 0 0 )   N O T   N U L L ,  
 	 [ M e t h o d N a m e ]   [ v a r c h a r ] ( 2 0 0 )   N O T   N U L L ,  
 	 [ S t a r t T i m e ]   [ d a t e t i m e ]   N O T   N U L L ,  
 	 [ E n d T i m e ]   [ d a t e t i m e ]   N O T   N U L L ,  
 	 [ E l a p s e d T i m e M S e c s ]   [ i n t ]   N O T   N U L L ,  
 	 [ A n n o t a t i o n ]   [ v a r c h a r ] ( 5 0 0 )   N O T   N U L L ,  
   C O N S T R A I N T   [ P K _ P E R F _ T i m i n g D a t a ]   P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ T i m i n g D a t a I D ]   A S C  
 ) W I T H   ( P A D _ I N D E X     =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E     =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S     =   O N ,   A L L O W _ P A G E _ L O C K S     =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
 G O  
 S E T   A N S I _ P A D D I N G   O F F  
 G O  
 A L T E R   T A B L E   [ d b o ] . [ P E R F _ S y s t e m I n f o ]     W I T H   C H E C K   A D D     C O N S T R A I N T   [ F K _ P E R F _ S y s t e m I n f o _ P E R F _ T e s t S u i t e R e s u l t ]   F O R E I G N   K E Y ( [ T e s t S u i t e R e s u l t I D ] )  
 R E F E R E N C E S   [ d b o ] . [ P E R F _ T e s t S u i t e R e s u l t ]   ( [ T e s t S u i t e R e s u l t I D ] )  
 O N   D E L E T E   C A S C A D E  
 G O  
 A L T E R   T A B L E   [ d b o ] . [ P E R F _ S y s t e m I n f o ]   C H E C K   C O N S T R A I N T   [ F K _ P E R F _ S y s t e m I n f o _ P E R F _ T e s t S u i t e R e s u l t ]  
 G O  
 A L T E R   T A B L E   [ d b o ] . [ P E R F _ T e s t R e s u l t ]     W I T H   C H E C K   A D D     C O N S T R A I N T   [ F K _ P E R F _ T e s t R e s u l t _ P E R F _ T e s t S u i t e R e s u l t ]   F O R E I G N   K E Y ( [ T e s t S u i t e R e s u l t I D ] )  
 R E F E R E N C E S   [ d b o ] . [ P E R F _ T e s t S u i t e R e s u l t ]   ( [ T e s t S u i t e R e s u l t I D ] )  
 O N   D E L E T E   C A S C A D E  
 G O  
 A L T E R   T A B L E   [ d b o ] . [ P E R F _ T e s t R e s u l t ]   C H E C K   C O N S T R A I N T   [ F K _ P E R F _ T e s t R e s u l t _ P E R F _ T e s t S u i t e R e s u l t ]  
 G O  
 A L T E R   T A B L E   [ d b o ] . [ P E R F _ T i m i n g D a t a ]     W I T H   C H E C K   A D D     C O N S T R A I N T   [ F K _ P E R F _ T i m i n g D a t a _ P E R F _ T e s t R e s u l t ]   F O R E I G N   K E Y ( [ T e s t R e s u l t I D ] )  
 R E F E R E N C E S   [ d b o ] . [ P E R F _ T e s t R e s u l t ]   ( [ T e s t R e s u l t I D ] )  
 O N   D E L E T E   C A S C A D E  
 G O  
 A L T E R   T A B L E   [ d b o ] . [ P E R F _ T i m i n g D a t a ]   C H E C K   C O N S T R A I N T   [ F K _ P E R F _ T i m i n g D a t a _ P E R F _ T e s t R e s u l t ]  
 G O  
 