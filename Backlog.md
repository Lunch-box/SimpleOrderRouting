SOR Backlog
===========

// Refactoring:

+ Transform IEnumerable<OrderDescription>  to a composite (OrderDescriptions) and giving orders responsibilities (typically implementing IOrder)
  - The composite will handle all notifications
  
+ Check events unsubscriptions 

+ Add acceptance with market rejection

+ Implement asynchronous nature of the markets

+ Acceptance test harness