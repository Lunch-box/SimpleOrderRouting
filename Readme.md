Simple Order Routing (SOR)
=========================

Prototype of a Financial (Smart) Order Routing system. The opportunity to explore various technical approaches (Reactive Programming, Sequencer, LMAX Disruptor, Event sourcing, ...).


What is a SOR?
--------------

A Smart Order Routing is a process used in trading application to execute orders for financial instruments (*) into various venues/markets/exchanges by following algorithms and execution strategies ensuring best execution, minimal market impact and reduced execution costs.

(*): example of financial instruments: currencies, securities, commodities, derivatives, futures, options,...

__[Click here](./UbiquitousLanguage.md)__ to learn more about SOR concepts and the ubiquitous language of our project.


What's the point with this spike project?
-----------------------------------------
Our goal is __NOT__ to implement SOR (business) algorithms. Our goal is to spike and compare various ways of implementing reactive applications with differents input and output types. In that case, we just pick the "meta" concept of Smart Order Routing to illustrate our explorations (with market data, order operations, execution reports, etc).

![SORBigPicture](https://raw.githubusercontent.com/Lunch-box/SimpleOrderRouting/master/images/SOR-bigPicture.jpg)


Our approach
------------
Even if this is a spike, we will try to implement it following some principles:
+ __Test Driven__: Every new feature is added following the TDD principles (testing behaviours) __and targeting 100% Test coverage__
+ __[YAGNI](http://en.wikipedia.org/wiki/You_aren't_gonna_need_it)__: following this XP practice, we don't add any functionality until deemed necessary
+ __[Clean Architecture](http://blog.8thlight.com/uncle-bob/2011/11/22/Clean-Architecture.html)__: every architecture choice is motivated, and allows us to defer critical decisions for the project
+ __"Don't guess; measure"__: we stick to this rule in order to avoid any kind of confirmation bias and *Premature optimization* (cause remember: "it's the root of all evil" ;-)
+ __Low ceremony__: we focus on efficiency (to deliver software) in our spike iterations. Normal, if we consider the fact that we work on that project mostly at __Lunch time__ (indeed, __we pair at noon__ to to build stuff).

Our journeys
------------
+ __Journey1__: our goal for this first journey is to familiarize ourselves with the main concepts of SOR, and to implement a first and very simple SOR (mono-threaded, capable to cope with one execution strategy simultaneously, with everything located within the same .dll, etc.)

+ __Journey2__: (TBD; but probably to make the SOR vertically scalable, allowing it to cope with multiple execution strategies simultaneously and with a proper performance test harness)

Disclaimer
----------
Since we pair most of the (lunch) time, the identity of the commiter is not very relevant.

- - -

__Thomas & Tomasz__ (Lunch-boxers)









