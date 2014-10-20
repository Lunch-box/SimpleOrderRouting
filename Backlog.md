SOR Backlog
===========

+ Null object pattern instead of nulls

+ Implement asynchronous nature of the markets (for their feedback)

+ Create our acceptance test harness (a console app generating report after each runs session)

+ Try various technical strategy for the inside-the-hexagon implementation (LMAX, F#, Michonne, ...)

+ Conclude and call for action for other guys/languages/platforms...


- - - 

## Events autours du passage d'ordre
Cf photo Cyrille
1, 2, 3, 4 dans presque tous les ordres possibles (race cond fonctionnelles)

1. Order updated: Canceled, executed, ... new status

	1. __Exec notif__: a quel prix tu as traité effectivement

2. Trade Info (traded):  avec qui? marges y/no ? settlement condition? (sous quels termes)

3. Last traded price: mise à jour du flux (dernier prix traité par instrument) -> le cours (last executed price)

4. __Feed Updated__: mise à jour du flux, avec impact sur les quantités par exemple et les prix 



Le solver est intéressé par 3 et 4

