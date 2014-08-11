Simple Order Routing (SOR)
=========================

Prototype of a Financial (Smart) Order Routing. The opportunity to explore various technical approaches (Reactive Programming, Sequencer, LMAX Disruptor, Event sourcing, ...).


What is a SOR?
--------------
Order routing is the process by which an order goes from the end user to an exchange. An order may go directly to the exchange from the customer, or it may go first to a broker who then routes the order to the exchange.

"Smart" order routing attempts to achieve best execution of trades while minimizing market impact. It is designed to help firms in an increasingly fragmented market to search for hidden liquidity, find opportunities in dark pools and use algorithms to maximize results without moving the market. (source: [MarketsWiki](http://marketswiki.com/mwiki/Order_routing))

A Smart Order Routing is a process used in trading application to execute incoming liquidity into liquidity providers following routing rules. The routing rules usually follow business needs like best execution or internalization.


What's the point with this spike project?
-----------------------------------------
Our goal is __NOT__ to implement SOR (business) algorithms. Our goal is to spike and compare various ways of implementing reactive applications. In that case, we just pick the "meta" concept of Smart Order Routing.

Our approach
------------
Even if this is a spike, we will implement it following some principles and guidelines
- __Test coverage__: 100%?
- __StyleCop__: <100%?>
- __[YAGNI](http://en.wikipedia.org/wiki/You_aren't_gonna_need_it)__: following this XP practice, we won't add any functionality until deemed necessary
- __Low ceremony__: we will try to focus on efficiency in our spike iterations








