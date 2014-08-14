Ubiquitous language
===================

Traders ask the Smart Order Router (SOR for short) to execute orders of 2 types:

- __Market order:__ simple order (1 leg only?) associated to a given venue/market/exchange

- __Trading order:__ more complex order that is usually transformed into a set of Market Orders executed in one or various venues/markets/exchanges. The number of market orders and the choice of their venue depends on the market conditions, affinities that we may have with venues, and the algorithms we enable within the SOR.

- - -

Trading Order types
-------------------

- __Simple order:__ one leg order (always matching with a Market Order?)
- __OCO ([One-Cancels-the-Other Order](http://www.investopedia.com/terms/o/oco.asp)):__ 2 legs order. First leg is a *[Take Profit Order](http://www.investopedia.com/terms/t/take-profitorder.asp)* and the other: a *[Stop Loss Order](http://www.investopedia.com/terms/s/stop-lossorder.asp)*
- __Entry:__ 3 legs [Conditional Order](http://www.investopedia.com/university/intro-to-order-types/conditional-orders.asp). (if entry leg is done ==> OCO)
- __EntryLoop:__ ???
- __TWAP:__ ???


Market Order type(s)
--------------------

- __Spot Simple Order__


Execution strategies of a SOR
-----------------------------
- GTC (Good 'till Cancel)
- IOC (Immediate or Cancel)
- Simple Order
