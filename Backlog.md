# SOR Backlog

+ Review the stack overflow exception when events from the MarketsAdapter are raised the other way round
+ Review the responsibilities between the SOREngine and the InstructionExecutionContext
+ Check that all adapters don't have any business logic
+ Review all the type to prevent from memory leak due to open event/callback subscriptions.
+ Split the MarketGatewaysAdapter into various types

---

Reminder: 

Refactoring intéressant mais qui montre qu'on a un peu du code spaghetti avec nos events et nos abstractions

On voulait encapsuler l'instruction de l'investisseur dans notre domain -> ça nous aurait fait bouger les événements de celle-ci sur le SOR unique -> cela aurait forcé tout le monde à tout recevoir tout le temps et filter (bof) -> On est parti sur une API dans laquelle on enregistre nos call backs

Pour l'instant (baby steps), on a introduit une méthode Subscribe, mais ce sera probablement à faire au niveau de la méthode Route(id, success, failure)

see. Railway programming (http://fsharpforfunandprofit.com/posts/recipe-part2/)

Question pour les experts du domain: 
+ Should all investors be aware of the other investors instruction notifications?
  - if yes: should we provide it by default or as an option?

Note: on a fait disparaitre le concept d'instrument au passage (au lieu de le laisser null) -> dommage

- - -


1. Migrate Market into another project as ExternalMarket

1. To introduce IMarket/Market for indomain use

1. To introduce the concept of (financial) instrument (mandatory to support the concept of *Market data* feeds)

1. To introduce the notion of market data and the MarketData API & Port/Adapter (starting from our acceptance tests)

1. To introduce the *OrderRouting* API & corresponding port/adapter

1. To create our acceptance test harness (i.e. a console application generating a report after each runs session)


- - -

## Remarks
+ Null object pattern instead of nulls

+ Implement asynchronous nature of the markets (for their feedback)

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

