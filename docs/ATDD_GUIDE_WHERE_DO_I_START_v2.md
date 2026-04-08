# "Valentina, where do I start?" - Revised version
### By Valentina Jemuovic - Optivem (with honest corrections)

---

Felix, you made a fair criticism. I'm going to be honest with you: in my first response I oversimplified some things and in others I was simply wrong. Let's correct that.

---

## What I stand by from my first response

The central message is still correct: **start with behavior, not with technical layers**. Thinking "should I start with the UI or with the database?" is the wrong question. "What should the system be able to do?" is the right one.

I also stand by the idea that the domain should emerge from usage, not be designed in a vacuum. And that naming acceptance tests as a roadmap from day 1 is useful.

But there are things I got wrong, and things I omitted. Let's get to it.

---

## Correction 1: What I called "Outside-In" is not Outside-In

I have to be honest here. When I said that the Use Case is "the outer boundary" and that starting there is outside-in, I was **redefining the term** to fit my proposal. And that's not correct.

### What Outside-In really is

In classic Outside-In TDD (Freeman & Pryce in "Growing Object-Oriented Software Guided by Tests", Sandro Mancuso, the London School), **"outside" literally means the entry point of the user or the client**. The acceptance test starts from where the external actor interacts with the system: the HTTP endpoint, the user interface, a bus message.

The reason is deliberate: you want your **interface design to emerge from how it's consumed**, not just from what it does internally.

```
GENUINE OUTSIDE-IN (Freeman & Pryce, London School):

  External actor
       |
       v
  +---------+     +-----------+     +----------+     +--------+
  |   API   | --> | Use Case  | --> |  Domain  | --> |  Repo  |
  | (test)  |     |  (test)   |     |  (test)  |     | (test) |
  +---------+     +-----------+     +----------+     +--------+
  Start            Discovered        Emerges from     Implemented
  HERE             from the API      the use case     at the end
                   test


WHAT I PROPOSED (which is actually MIDDLE-OUT):

                  +-----------+     +----------+
                  | Use Case  | --> |  Domain  |
                  |  (test)   |     |  (test)  |
                  +-----------+     +----------+
                  Start              Emerges from
                  HERE               the use case

                  Then add API and persistence
```

**What I described as "School B" is middle-out.** And middle-out is perfectly valid as an approach, but it shouldn't be sold as outside-in. That creates conceptual confusion.

### When to use each approach

| Approach | When it's appropriate |
|---|---|
| **Genuine Outside-In** | When the API/interface design matters, when you work with real systems, when you need early feedback from infrastructure |
| **Middle-Out (Use Case first)** | When you're exploring business logic, when the delivery interface isn't defined yet, as a learning exercise |
| **Inside-Out** | When the domain is very complex and you need to model it first (e.g.: rules engine, algorithms) |

None is superior. They are tools for different contexts.

---

## Correction 2: "Tests coupled to use cases, not to domain classes" needs nuance

I said:

> *"Unit tests should be coupled to use cases, NOT to internal domain classes."*

This is dangerous without context. It needs important nuance.

### When it's correct to test only at the use case level

When the domain logic is simple and is naturally covered from the use case:

```csharp
// The use case test covers the Account.Deposit logic implicitly
[Fact]
public void should_transfer_money_between_accounts()
{
    // ... setup ...
    _useCase.Execute("ACC-001", "ACC-002", amount: 200m);
    
    _accountRepository.FindById("ACC-001").Balance.Should().Be(800m);
    _accountRepository.FindById("ACC-002").Balance.Should().Be(700m);
}
```

Here you don't need a separate test for `Account.Deposit()` because the use case test already covers it.

### When you DO NEED domain tests directly

When your domain has **complex logic** that would be combinatorially explosive to cover from the use case:

```csharp
// Example: interest calculation with multiple rules
public class InterestCalculatorTest
{
    [Theory]
    [InlineData(1000, 12, AccountType.Savings, 30.00)]    // 3% annual
    [InlineData(1000, 12, AccountType.Premium, 45.00)]    // 4.5% annual
    [InlineData(5000, 6, AccountType.Savings, 75.00)]     // 3% * 6 months
    [InlineData(5000, 6, AccountType.Premium, 137.50)]    // with bonus tier
    [InlineData(100000, 12, AccountType.Premium, 5500)]   // high-value rate
    public void should_calculate_interest_correctly(
        decimal balance, int months, AccountType type, decimal expectedInterest)
    {
        var calculator = new InterestCalculator();
        var result = calculator.Calculate(balance, months, type);
        result.Should().Be(expectedInterest);
    }
}
```

```csharp
// Example: booking state machine
public class BookingLifecycleTest
{
    [Fact] public void pending_booking_can_be_confirmed() { ... }
    [Fact] public void pending_booking_can_be_cancelled() { ... }
    [Fact] public void confirmed_booking_cannot_be_confirmed_again() { ... }
    [Fact] public void cancelled_booking_cannot_be_confirmed() { ... }
    [Fact] public void confirmed_booking_can_be_checked_in_on_start_date() { ... }
    [Fact] public void confirmed_booking_cannot_be_checked_in_before_start_date() { ... }
}
```

```csharp
// Example: policy validation with combinatorics
public class BookingPolicyTest
{
    [Fact] public void employee_policy_allows_room_in_list() { ... }
    [Fact] public void employee_policy_denies_room_not_in_list() { ... }
    [Fact] public void company_policy_applies_when_no_employee_policy() { ... }
    [Fact] public void employee_policy_takes_precedence_over_company_policy() { ... }
    [Fact] public void no_policies_means_all_rooms_allowed() { ... }
    [Fact] public void empty_employee_policy_denies_everything() { ... }
}
```

Trying to cover all these combinations from a use case test would be a disaster: each test would need repository setup, creation of auxiliary entities, and the failure reason would be opaque.

### The correct rule

> **Test behavior at the most external level that is practical, and go down a level when domain complexity justifies it.**

This means:
- **Use case tests** for the general flow and interactions between components
- **Domain tests** for complex logic, calculations, state machines, combinatorial validations
- **Don't duplicate**: if the use case test already covers a case, don't repeat it in the domain

In your Corporate Hotel Booking, the commit `1dbfdd4` where you found a bug by testing `Hotel.SetRoom` directly is a **perfect example** of why you need domain tests. The use case test had not detected that bug.

---

## Correction 3: Mocks vs Fakes - the real criterion

I said "mocks in the controller, fakes in the use case". But I didn't explain the *why*. The criterion isn't "by layer". It's by **what you're verifying**.

### State verification vs interaction verification

| Technique | When to use it | What you verify |
|---|---|---|
| **Fake + assert state** | When the observable result matters | "The balance changed to 800" |
| **Mock + verify** | When the interaction itself is the contract | "The use case was called with these parameters" |

### Concrete examples

**Fake (you verify resulting state):**
```csharp
// Use case test: what matters is WHAT HAPPENED (the balance changed)
var fakeRepo = new InMemoryAccountRepository();
fakeRepo.Add(Account.With(id: "ACC-001", balance: 1000m));

_useCase.Execute("ACC-001", "ACC-002", 200m);

fakeRepo.FindById("ACC-001").Balance.Should().Be(800m);  // resulting state
```

**Mock (you verify interaction):**
```csharp
// Controller test: what matters is THAT IT DELEGATED correctly
_useCaseMock.Setup(x => x.Execute("ACC-001", "ACC-002", 200m))
    .Returns(TransferResult.Success());

var response = _controller.Transfer(request);

response.Should().BeOfType<OkResult>();  // correct mapping
// The controller is pure delegation, it has no state of its own to verify
```

**Mock (you verify a critical side effect):**
```csharp
// Sometimes a mock is correct even in a use case
// Example: verify that a notification was sent
_notificationService.Verify(x => x.Send(
    It.Is<TransferNotification>(n => 
        n.FromAccount == "ACC-001" && 
        n.Amount == 200m)), 
    Times.Once);
// The notification is an observable side effect that you can't
// verify with state (there's no "notification repository")
```

### The real rule

It's not "mocks in controllers, fakes in use cases". It's:

> **Use fakes when you can verify resulting state. Use mocks when you need to verify that an interaction occurred (because there's no observable state to inspect).**

A controller has no state of its own - it only delegates and maps. That's why you use mocks. But if your use case needs to verify that an email was sent, a mock of the email service is perfectly valid.

---

## Correction 4: The onion diagram was wrong

I put infrastructure at the center of the onion. That contradicts all of hexagonal architecture. The correct diagram is:

```
         +----------------------------------------------+
         |  INFRASTRUCTURE (DB, External APIs, Email)   |  <- Outer ring
         |  +--------------------------------------+    |
         |  |  UI / API  (delivery mechanism)      |    |  <- Outer ring
         |  |  +------------------------------+    |    |
         |  |  |  USE CASES  (application)    |    |    |  <- Middle ring
         |  |  |  +----------------------+    |    |    |
         |  |  |  |  DOMAIN (the center) |    |    |    |  <- THE CENTER
         |  |  |  +----------------------+    |    |    |
         |  |  +------------------------------+    |    |
         |  +--------------------------------------+    |
         +----------------------------------------------+

    Dependency rule: arrows ALWAYS point inward.
    The domain depends on nothing. Everything depends on the domain.
```

**UI, API and infrastructure are in the same outer ring.** Both are "details" in Clean Architecture terms. Both depend on the domain, never the other way around.

---

## Correction 5: The Walking Skeleton - what I omitted

This is the most serious omission. I didn't mention the **Walking Skeleton** and it's probably the most practical concept for answering "where do I start".

### What is a Walking Skeleton

A concept from Alistair Cockburn (revisited by Freeman & Pryce): your **first step** should be a **complete vertical slice** -- from the entry point to persistence -- that works end-to-end even if it does something trivial.

```
        Walking Skeleton: a complete vertical slice

  +----------+    +-----------+    +----------+    +----------+
  |   POST   |    | AddHotel  |    |  Hotel   |    |  SQL DB  |
  | /hotels  |--->| UseCase   |--->| (entity) |--->| (table)  |
  |          |    |           |    |          |    |          |
  |   201    |<---|  result   |<---|          |<---|  SELECT  |
  +----------+    +-----------+    +----------+    +----------+
  
  Real              Minimal          Minimal         Real
  endpoint          logic            domain          persistence

  EVERYTHING works end-to-end. It does something trivial (create a hotel
  with a name). But it traverses ALL layers.
```

### Why it's critical

Without a walking skeleton, you can have 15 use cases working with InMemory repos and discover late that:

- Your domain model doesn't persist well (relationships, types, mappings)
- Your API has serialization issues (DateTimes, GUIDs, nullables)
- Your infrastructure has unexpected friction (migrations, connections, timeouts)
- Your DI container doesn't know how to wire everything together

**The walking skeleton gives you feedback about the real architecture**, not just about the logic in memory.

### What it looks like in practice for Corporate Hotel Booking

```
Day 1, step 1: Walking Skeleton

  Acceptance test:
    POST /hotels with {hotelId, hotelName}
    -> Returns 201
    GET /hotels/{hotelId}
    -> Returns the hotel with its name

  Minimal implementation:
    - HotelsController (real)
    - AddHotelUseCase (real, trivial)
    - Hotel entity (only id + name)
    - IHotelRepository (interface)
    - InMemoryHotelRepository (so the test passes quickly)
    - SqlHotelRepository (to verify that infra works)
    
  Result: an E2E test that traverses the entire stack.
  It doesn't do anything interesting, but IT PROVES EVERYTHING IS CONNECTED.
```

### Walking Skeleton + Outside-In: the complete flow

```
1. WALKING SKELETON
   Trivial E2E acceptance test (RED)
   -> Controller -> UseCase -> Domain -> Repo -> DB
   Everything minimal, but everything real and connected (GREEN)

2. FIRST REAL FEATURE (double loop)
   Acceptance test: "employee books available room" (RED)
   
   Inner TDD loop:
     a. Controller test -> expand controller
     b. Use Case test -> implement logic
     c. Domain test (if there's complexity) -> model
     d. Integration test -> expand repo
   
   Acceptance test passes (GREEN)
   Refactor

3. NEXT FEATURE (double loop)
   Acceptance test: "booking rejected by policy" (RED)
   ... same pattern ...
```

The walking skeleton is not an "extra". It's **step 0** that gives foundation to everything else.

---

## The corrected flow: how I would actually start

```
TIME ---------------------------------------------------------------->

 0. WALKING SKELETON         Trivial vertical slice.
    |                        Endpoint -> UseCase -> Domain -> DB.
    |                        EVERYTHING real, EVERYTHING connected, EVERYTHING minimal.
    |                        It gives you confidence in the architecture.
    |
    v
 1. ACCEPTANCE TEST          First real user journey (RED).
    | (outer loop)           From the entry point (API/UI).
    |                        Business language, not HTTP.
    |
    |  +----------------------------------------------+
    |  |  INNER TDD LOOP                              |
    |  |                                              |
    |  |  2a. Controller test -> Controller            |
    |  |      (mock the use case, verify mapping)     |
    |  |                                              |
    |  |  2b. Use Case test -> Use Case               |
    |  |      (fake repos, verify behavior)           |
    |  |                                              |
    |  |  2c. Domain test -> Entities                 |
    |  |      (ONLY if there's complex logic:         |
    |  |       calculations, state machines,          |
    |  |       combinatorial rules)                   |
    |  |                                              |
    |  |  2d. Integration test -> Repository          |
    |  |      (real persistence)                      |
    |  +----------------------------------------------+
    |
    v
 3. ACCEPTANCE TEST PASSES   The outer loop closes (GREEN).
    |
    v
 4. REFACTOR                 Cleanup, patterns, naming.
    |
    v
 5. NEXT FEATURE             Repeat from step 1.
```

---

## Applied to your repo: what would have changed

### Your Phase 1-2 (the false start)

**What you did:** You started with HTTP, removed it, did middle-out with HotelService, step back, restart.

**What I would have done with a walking skeleton:**
```
Commit 1: test (red): walking skeleton - POST /hotels returns 201
Commit 2: feat (green): minimal controller + use case + in-memory repo
Commit 3: test: verify SQL persistence works for hotel (integration)
           -> At this point you already know your full stack works
Commit 4: test (red): acceptance - employee books available room (user journey)
Commit 5-N: inner TDD loop until the acceptance test passes
```

The walking skeleton would have saved you the false start because:
- You wouldn't have needed to remove the API ("remove API implementation to simplify")
- You would have discovered the shape of the controller from day 1
- You would have validated real persistence at the beginning, not in Phase 6

### Your Phase 4 (Policies - your best moment)

You already did this well. The sequence Acceptance -> Controller -> UseCase -> Repository is genuine outside-in. The only thing I would change is adding domain tests for the policy logic, where there's combinatorics:

```csharp
// These are tests I found you're missing
[Fact] public void employee_policy_takes_precedence_over_company() { }
[Fact] public void empty_employee_policy_denies_all_rooms() { }
[Fact] public void no_policy_at_all_allows_all_rooms() { }
```

Your `IsBookingAllowedUseCaseTest` covers some of these, but the full combinatorics would be clearer in a direct domain test.

---

## Summary: the 5 corrections

| # | What I said | What's correct |
|---|---|---|
| 1 | "Starting with use case is outside-in" | It's **middle-out**. Genuine outside-in starts from the external actor's entry point. Both are valid, but they are different things. |
| 2 | "Tests only at the use case level" | Tests at the most external level that is **practical**. Go down to the domain when there's complexity (calculations, state machines, combinatorics). |
| 3 | "Mocks in controllers, fakes in use cases" | **Fakes when you verify state**, **mocks when you verify interaction**. It's not by layer, it's by what you need to check. |
| 4 | Infra at the center of the onion | Infra in the **outer ring**, same as UI/API. Domain at the center. |
| 5 | Didn't mention walking skeleton | The walking skeleton is **step 0**. A trivial vertical slice that validates the entire architecture is connected before building features. |

---

## For your real-world context (beyond the kata)

In a real project with legacy code, complex infrastructure and interacting components:

1. **The walking skeleton is not optional** -- it's what saves you from discovering infrastructure problems in week 8.

2. **Integration tests are not "the last thing"** -- they are part of the feedback loop from early on. The kata lets you postpone them because InMemory works fine. In production, a SQL query that doesn't JOIN correctly can ruin your day.

3. **The "everything with InMemory fakes" approach has a practical limit** -- it's fantastic for business logic, but it doesn't replace validating that your system works with real infrastructure.

4. **Genuine outside-in protects you from something that middle-out doesn't**: if you start with the use case, you can design a perfect internal interface that is later awkward to expose via API. If you start with the API, the interface design is guided by the consumer.

> *Felix, I was wrong to oversimplify, and thank you for not letting it slide. The nuance matters. Outside-in, middle-out, inside-out -- they are tools, not religions. What is non-negotiable: start with behavior, have a walking skeleton early, and test at the appropriate level based on complexity. Everything else is a context decision.*
>
> *-- Valentina (corrected and without excuses)*
