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
Our goal is __NOT__ to implement SOR (business) algorithms. Our goal is to spike and compare various ways of implementing reactive applications. In that case, we just pick the "meta" concept of Smart Order Routing to illustrate our explorations.

Our approach
------------
Even if this is a spike, we will try to implement it following some principles:
+ __Test Driven__: Every new feature is added following the TDD principles (testing behaviours) __and targeting 100% Test coverage__
+ __[YAGNI](http://en.wikipedia.org/wiki/You_aren't_gonna_need_it)__: following this XP practice, we don't add any functionality until deemed necessary
+ __[Clean Architecture](http://blog.8thlight.com/uncle-bob/2011/11/22/Clean-Architecture.html)__: every architecture choice is motivated, and allows us to defer critical decisions for the project
+ __Low ceremony__: we focus on efficiency (to deliver software) in our spike iterations
+ __"Don't guess; measure"__: we stick to this rule in order to avoid any kind of confirmation bias and *Premature optimization* (cause remember: "it's the root of all evil" ;-)
+ __Lunch time__: as any other Lunch-box project, we pair at noon to to build stuff.

- - -

__Thomas & Tomasz__ (Lunch-boxers)









