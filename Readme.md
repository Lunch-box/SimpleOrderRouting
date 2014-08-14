Simple Order Routing (SOR)
=========================

Prototype of a Financial (Smart) Order Routing. The opportunity to explore various technical approaches (Reactive Programming, Sequencer, LMAX Disruptor, Event sourcing, ...).


What is a SOR?
--------------
__Order routing__ is the process by which an order goes from the end user to an exchange. An order may go directly to the exchange from the customer, or it may go first to a broker who then routes the order to the exchange.

__"Smart" order routing__ attempts to achieve best execution of trades while minimizing market impact. It is designed to help firms in an increasingly fragmented market to search for hidden liquidity, find opportunities in dark pools and use algorithms to maximize results without moving the market. (source: [MarketsWiki](http://marketswiki.com/mwiki/Order_routing))

From an IT point of view, a Smart Order Routing is a process used in trading application to execute incoming liquidity into liquidity providers following routing rules. The routing rules usually follow business needs like best execution or internalization.

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
+ __Journey1__: our goal for this first journey is to familiarize with the main concepts of SOR, and to implement a first and very simple SOR (mono-threaded, capable to cope with one execution strategy simultaneously, with everything located within the same .dll, etc.)

+ __Journey2__: (TBD; but probably to make the SOR vertically scalable, allowing it to cope with multiple execution strategies simultaneously and with a proper performance test harness)

Disclaimer
----------
Since we pair most of the time, the identity of the commiter is not very relevant here.

- - -

__Thomas & Tomasz__ (Lunch-boxers)









