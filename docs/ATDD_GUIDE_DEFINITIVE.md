# Corporate Hotel Booking Kata — Outside-In ATDD Guide (Definitive)

**A distilled guide that integrates the best of Valentina Jemuovic, the critical corrections from Freeman & Pryce / London School, and the lessons learned from this repository itself.**

---

## Context

This guide is born from three sources:

1. **Valentina Jemuovic's original guide (Optivem)** — with her focus on Use Case Driven Development, the 4-layer architecture for acceptance tests, and the principle of "think in behaviors, not in layers".

2. **The critical corrections** — where conceptual errors were identified: redefining "outside-in" as middle-out, treating the walking skeleton as a throwaway test instead of as the first real journey, absolutizing "tests only at use case level", choosing mock/fake by layer instead of by criteria, and the inverted onion diagram.

3. **The repository analysis** — 78 commits that show a real learning process, with a false start, a key pivot, and a Phase 4 (Policies) where outside-in was applied genuinely.

What follows is the final synthesis: what works, corrected where it fails, and concretely applied to this kata.

---

## Fundamental Principles

### 1. Think in behaviors, not in layers

When you ask yourself "do I start with the UI, the API, the domain, or the database?", the question is poorly framed. The correct question is: **"what should the user be able to do?"**

That is a behavior. A journey. A business scenario.

> *This principle comes from Valentina and is correct. It is the mental starting point.*

### 2. Outside-In means Outside-In

The "outside" is where the external actor interacts with the system: the HTTP endpoint, the CLI command, the UI. It is not the Use Case.

The Use Case is the heart of the business logic, but it is not "outside". Starting from the Use Case is **middle-out**, which is valid but should not be called outside-in.

In genuine outside-in (Freeman & Pryce, Sandro Mancuso, London School):

```
  External actor
       |
       v
  +---------+     +-----------+     +----------+     +--------+
  |   API   | --> | Use Case  | --> |  Domain  | --> |  Repo  |
  |  (test) |     |  (test)   |     |  (test)  |     | (test) |
  +---------+     +-----------+     +----------+     +--------+
  Start            Discovered        Emerges from     Implemented
  HERE             from the API      the use case     at the end
                   test
```

The design of your interfaces emerges from **how they are consumed**, not from how you model them internally.

### 3. Walking Skeleton: your first journey IS the skeleton

A walking skeleton (Cockburn) is "the thinnest possible slice of real functionality that can be deployed end-to-end." It is real production code — routing, controller, use case, repository — that works end-to-end even though it does something trivial.

**The walking skeleton is not a throwaway test.** It is what emerges from implementing your first real journey with outside-in discipline.

The key is to choose a **journey so simple that it acts as a walking skeleton**: trivial business logic, but that forces you to set up the complete pipeline. The references treat it this way:

| Reference | Approach |
|---|---|
| **Freeman & Pryce (GOOS)** | First real E2E acceptance test (not throwaway). In their example (auctions), the test verifies "the system connects and receives a message." Intentionally boring as business, but traverses the entire stack. **It stays in the suite.** |
| **Sandro Mancuso** | Starts directly with the first real business acceptance test, but chooses the simplest possible journey. The walking skeleton emerges as a side effect. **It is not a separate step.** |
| **Cockburn** (who coined the term) | Treats it as an architectural activity: make the complete deploy work. Does not prescribe a specific type of test. |
| **Kent Beck** | Does not talk about walking skeleton. Starts with the simplest test that can fail (more bottom-up). |

**Sandro's approach is the cleanest for this kata:** your first acceptance test IS real business, but of the simplest journey. "Add a hotel" is perfect: it is real business (not thrown away), it is trivial as logic, and it forces you to set up pipeline + DSL + driver in their most minimal form.

### 4. Test at the most external level that is practical

- For general flows: **Use Case tests** with fakes
- For logic with combinatorics (dates, policies, multidimensional rules): **Domain tests** directly
- For HTTP mapping: **Controller tests** with mocks
- For persistence: **Integration tests** with a real database

Don't test everything from the use case nor everything from the domain. Choose the level according to the complexity.

### 5. Mock vs Fake: the criterion is what you verify, not the layer

| You verify... | Tool | Example |
|---|---|---|
| That A delegated to B correctly | **Mock** | Controller -> Use Case |
| Resulting state of an operation | **Fake** | "There is 1 booking in the repo" |
| Pure logic without dependencies | **Neither mock nor fake** | `DateRange.Overlaps()` |

---

## The Double Loop

```
+============================================================+
|  OUTER LOOP (ATDD)                                          |
|                                                             |
|  1. Acceptance Test in RED                                  |
|     +--------------------------------------------------+   |
|     |  INNER LOOP (TDD)                                |   |
|     |                                                  |   |
|     |  2. Controller test (mock use case) -> impl      |   |
|     |  3. Use Case test (fake repos) -> impl           |   |
|     |  4. Domain test (if combinatorics) -> impl       |   |
|     |  5. Integration test (real repo) -> impl         |   |
|     +--------------------------------------------------+   |
|  6. Acceptance Test passes (GREEN)                          |
|  7. Refactor                                                |
|                                                             |
+============================================================+
```

The acceptance test tells you **when you are done**. The inner loop tells you **how to get there**.

---

## 4-Layer Test Architecture

Valentina's structure, adopted with the critical nuance of starting with HttpDriver:

```
+----------------------------------------------------------+
|  Layer 1: THE TEST                                       |
|  Pure business language. No technical details.           |
|                                                          |
|  "An employee can book an available Suite"               |
+----------------------------------------------------------+
|  Layer 2: THE DSL                                        |
|  Given / When / Then with fluent API.                    |
|                                                          |
|  Given.AHotelWith(rooms)                                 |
|  When.TheEmployeeBooks(suite, dates)                     |
|  Then.TheBookingIsConfirmed()                            |
+----------------------------------------------------------+
|  Layer 3: DRIVER PORT (Interface)                        |
|  Defines WHAT operations you need.                       |
|                                                          |
|  IHotelDriver, IBookingDriver, IEmployeeDriver           |
+----------------------------------------------------------+
|  Layer 4: DRIVER ADAPTER (Implementation)                |
|  Defines HOW you interact with the system.               |
|                                                          |
|  HttpHotelDriver    <- FIRST (genuine outside-in)        |
|  DirectHotelDriver  <- LATER (for speed)                 |
+----------------------------------------------------------+
```

**Key difference from Valentina:** you start with HttpDriver, not with DirectDriver. The HttpDriver forces you to design from the consumer. You add the DirectDriver later as a fast regression suite.

---

## Concrete Application: Corporate Hotel Booking

### Step 0: Journey Roadmap

Before coding, you define the behaviors:

```
JOURNEY 1: Configure hotel
  - A hotel can be added
  - Rooms can be configured
  - Duplicate hotel is rejected

JOURNEY 2: Manage employees
  - An employee can be added to a company
  - An employee can be deleted

JOURNEY 3: Employee books a room (happy path)
  - An employee books an available Suite

JOURNEY 4: Policy restrictions
  - Employee policy rejects non-allowed type
  - Company policy rejects non-allowed type
  - Employee policy takes precedence over company policy
  - Without policies, everything is allowed

JOURNEY 5: Availability restrictions
  - No rooms available for those dates
  - Rooms of the type exist even though others are occupied

JOURNEY 6: Validations
  - Room type that the hotel does not offer
  - CheckOut must be after CheckIn

JOURNEY 7: Cascades
  - Deleting an employee deletes their bookings and policies
```

### Step 1: Journey 1 — "Add a hotel" (the walking skeleton)

This is your first journey AND your walking skeleton at the same time. It is real business (not thrown away), trivial as logic (almost no sad paths), and it forces you to set up the entire pipeline + DSL + driver in their most minimal form.

#### Commit 1: Acceptance test in RED (does not compile)

```csharp
[Fact]
public void should_add_a_hotel()
{
    // Given
    var hotelId = Guid.NewGuid();

    // When
    Given.AHotelExists(hotelId, "Westing");

    // Then
    var hotel = Then.TheHotelExists(hotelId);
    hotel.Name.Should().Be("Westing");
}
```

There is no HTTP, JSON, or status codes. Just business. But for this test to pass you need: WebApplicationFactory, routing, serialization, controller, use case, repository, DSL, driver port, HTTP driver adapter. **The entire stack, forced by a real business test.**

#### Commit 2: Minimal scaffolding (compiles but fails)

```csharp
// Driver Port — in its most minimal form
public interface IHotelTestDriver
{
    void CreateHotel(Guid hotelId, string name);
    HotelInfo FindHotel(Guid hotelId);
}

// HTTP Driver Adapter — HTTP encapsulated here, invisible to the test
public class HttpHotelTestDriver : IHotelTestDriver
{
    private readonly HttpClient _client;

    public void CreateHotel(Guid hotelId, string name)
    {
        var response = _client.PostAsJsonAsync("hotels",
            new { HotelId = hotelId, HotelName = name }).Result;

        if (response.StatusCode != HttpStatusCode.Created)
            throw new TestDriverException($"Failed to create hotel: {response.StatusCode}");
    }

    public HotelInfo FindHotel(Guid hotelId)
    {
        var response = _client.GetAsync($"hotels/{hotelId}").Result;
        if (response.StatusCode != HttpStatusCode.OK)
            throw new TestDriverException($"Hotel not found: {response.StatusCode}");

        return response.Content.ReadFromJsonAsync<HotelInfo>().Result;
    }
}

// Minimal DSL
public class GivenContext
{
    private readonly IHotelTestDriver _hotelDriver;

    public void AHotelExists(Guid hotelId, string name)
        => _hotelDriver.CreateHotel(hotelId, name);
}

public class ThenContext
{
    private readonly IHotelTestDriver _hotelDriver;

    public HotelInfo TheHotelExists(Guid hotelId)
        => _hotelDriver.FindHotel(hotelId);
}
```

**Note:** DSL and drivers are born in their most minimal form. One method in Given, one method in Then, one driver with two operations. Everything you create stays. Nothing is throwaway. They will be expanded journey by journey.

#### Commit 3: Controller test (outside-in, first point of contact)

```csharp
[Fact]
public void ShouldAddHotel()
{
    // Arrange
    var hotelId = Guid.NewGuid();
    var hotelName = "Westing";

    // Act
    var response = _hotelsController.AddHotel(new AddHotelBody(hotelId, hotelName));

    // Assert
    _addHotelUseCaseMock.Verify(mock => mock.Execute(hotelId, hotelName), Times.Once);
    ((StatusCodeResult)response).StatusCode.Should().Be((int)HttpStatusCode.Created);
}
```

#### Commit 4: Controller implementation + minimal AddHotelUseCase + InMemoryRepo

#### Commit 5: FindHotel — controller test, use case test, implementation

#### Commit 6: Acceptance test passes (GREEN)

**Result:** The complete pipeline works. You have DSL, driver, controller, use case, repository. And your first permanent business test in the suite. From here on, each new journey expands the DSL and drivers incrementally.

#### Commit 7: Refactor

---

### Step 2: Journey 1 expanded — Hotel sad paths

With the skeleton working, you expand the journey:

```csharp
[Fact]
public void should_reject_duplicated_hotel()
{
    // Given
    var hotelId = Guid.NewGuid();
    Given.AHotelExists(hotelId, "Westing");

    // When / Then
    var action = () => Given.AHotelExists(hotelId, "Same hotel");
    action.Should().Throw<TestDriverException>(); // The driver translates 409 to exception
}
```

Here you expand the driver (to throw exception on Conflict), the controller test (duplicate case), and the use case test (ExistingHotelException). Each commit is a step.

---

### Step 3: Journey 3 — "Employee books a room" (first complex feature)

#### Commit 1: Acceptance test in RED (does not compile)

```csharp
[Fact]
public void employee_books_an_available_suite()
{
    // Given
    var hotelId = Given.AHotelExists("Westing");
    Given.TheHotelHasRooms(hotelId,
        Room.OfType("Suite").WithNumber(101),
        Room.OfType("Suite").WithNumber(102));
    var employeeId = Given.AnEmployeeExists("Acme Corp");

    // When
    var booking = When.TheEmployeeBooks(
        employeeId, hotelId,
        roomType: "Suite",
        checkIn: Today,
        checkOut: Today.AddDays(5));

    // Then
    Then.TheBookingIsConfirmed(booking);
    Then.TheBookingHas(booking, b =>
    {
        b.Hotel.Is(hotelId);
        b.BookedBy.Is(employeeId);
        b.RoomType.Is("Suite");
    });
}
```

**Note how the DSL already has `Given.AHotelExists`** from Journey 1. You only need to add the new methods (`TheHotelHasRooms`, `AnEmployeeExists`, `TheEmployeeBooks`, etc.). The DSL grows incrementally.

#### Commit 2: Scaffolding — expand DSL and drivers with new stubs

```csharp
// Expand Driver Ports
public interface IBookingTestDriver
{
    BookingResult Book(Guid employeeId, Guid hotelId,
        string roomType, DateTime checkIn, DateTime checkOut);
}

// HTTP Driver Adapter for bookings
public class HttpBookingTestDriver : IBookingTestDriver
{
    private readonly HttpClient _client;

    public BookingResult Book(Guid employeeId, Guid hotelId,
        string roomType, DateTime checkIn, DateTime checkOut)
    {
        var response = _client.PostAsJsonAsync("bookings", new
        {
            EmployeeId = employeeId,
            HotelId = hotelId,
            RoomType = roomType,
            CheckIn = checkIn,
            CheckOut = checkOut
        }).Result;

        if (response.StatusCode == HttpStatusCode.Created)
        {
            var dto = response.Content.ReadFromJsonAsync<BookingDto>().Result;
            return BookingResult.Confirmed(dto);
        }

        return BookingResult.Rejected(response.Content.ReadAsStringAsync().Result);
    }
}
```

#### Commit 3: Controller test (outside-in, first point of contact)

```csharp
[Fact]
public void returns_created_when_booking_succeeds()
{
    // Arrange — mock of the use case (we verify delegation)
    _bookUseCaseMock
        .Setup(x => x.Execute(It.IsAny<BookingCommand>()))
        .Returns(BookingConfirmation.With(bookingId, employeeId, hotelId, "Suite"));

    // Act
    var response = _controller.Book(request);

    // Assert
    response.Should().BeOfType<CreatedResult>();
}
```

#### Commit 4: Controller implementation

#### Commit 5: Use Case test (fake repos, we verify state)

```csharp
[Fact]
public void confirms_booking_when_room_is_available()
{
    // Arrange — fakes (we verify resulting state)
    var hotelRepo = new InMemoryHotelRepository();
    var bookingRepo = new InMemoryBookingRepository();
    hotelRepo.Add(Hotel.With("H1", "Westing",
        rooms: new[] { Room.Suite(101), Room.Suite(102) }));

    var useCase = new BookUseCase(hotelRepo, bookingRepo);

    // Act
    var result = useCase.Execute(new BookingCommand(
        employeeId: "E1", hotelId: "H1",
        roomType: "Suite", checkIn: Today, checkOut: Today.AddDays(5)));

    // Assert
    result.IsConfirmed.Should().BeTrue();
    bookingRepo.FindByEmployee("E1").Should().HaveCount(1);
}
```

#### Commit 6: Use Case implementation

#### Commit 7: Domain test (availability logic has combinatorics)

```csharp
[Fact]
public void all_suites_booked_in_overlapping_dates()
{
    var hotel = Hotel.With("H1", "Westing",
        rooms: new[] { Room.Suite(101) });
    var existingBookings = new[]
    {
        Booking.For("E1", "H1", "Suite", Today, Today.AddDays(3))
    };

    hotel.HasAvailableRoom("Suite", existingBookings, Today.AddDays(1), Today.AddDays(4))
        .Should().BeFalse();
}

[Fact]
public void suite_available_when_dates_dont_overlap()
{
    var hotel = Hotel.With("H1", "Westing",
        rooms: new[] { Room.Suite(101) });
    var existingBookings = new[]
    {
        Booking.For("E1", "H1", "Suite", Today, Today.AddDays(3))
    };

    hotel.HasAvailableRoom("Suite", existingBookings, Today.AddDays(5), Today.AddDays(8))
        .Should().BeTrue();
}
```

**Why domain test here:** date overlap has combinatorics (partial, contained, adjacent, no overlap). Covering it all from the use case would be an explosion of setup. Your own commit `1dbfdd4` where you found a bug in `Hotel.SetRoom` demonstrates exactly why you need these tests.

#### Commit 8: Domain implementation

#### Commit 9: Acceptance test passes (GREEN)

#### Commits 10-11: Refactor

```
12 commits for a journey. Each one atomic, revertible, tells a story.
```

### Step 4: Journey 4 — Policies (where the domain deserves its own tests)

#### Acceptance test

```csharp
[Fact]
public void employee_cannot_book_room_outside_their_policy()
{
    // Given
    var hotelId = Given.AHotelExists("Westing");
    Given.TheHotelHasRooms(hotelId, Room.OfType("Suite").WithNumber(101));
    var employeeId = Given.AnEmployeeExists("Acme Corp");
    Given.EmployeeHasPolicy(employeeId, allowedTypes: "Standard");

    // When — tries to book Suite, but can only book Standard
    var booking = When.TheEmployeeBooks(
        employeeId, hotelId, roomType: "Suite",
        checkIn: Today, checkOut: Today.AddDays(5));

    // Then
    Then.TheBookingIsRejected(booking);
}
```

#### Domain tests for policy priority logic

```csharp
public class BookingPolicyTests
{
    [Fact]
    public void employee_policy_takes_precedence_over_company_policy()
    {
        var company = CompanyPolicy.For("Acme", allowed: new[] { "Standard" });
        var employee = EmployeePolicy.For("E1", allowed: new[] { "Suite", "Standard" });

        var service = new BookingPolicyService();

        service.IsAllowed("E1", "Suite", employee, company)
            .Should().BeTrue();
    }

    [Fact]
    public void falls_back_to_company_when_no_employee_policy()
    {
        var company = CompanyPolicy.For("Acme", allowed: new[] { "Standard" });

        new BookingPolicyService()
            .IsAllowed("E1", "Suite", employeePolicy: null, company)
            .Should().BeFalse();
    }

    [Fact]
    public void allows_all_when_no_policies_exist()
    {
        new BookingPolicyService()
            .IsAllowed("E1", "Suite", employeePolicy: null, companyPolicy: null)
            .Should().BeTrue();
    }

    [Fact]
    public void empty_employee_policy_denies_everything()
    {
        var employee = EmployeePolicy.For("E1", allowed: Array.Empty<string>());

        new BookingPolicyService()
            .IsAllowed("E1", "Suite", employee, companyPolicy: null)
            .Should().BeFalse();
    }
}
```

**This combinatorics would be a disaster from the use case test.** Each case would need to create hotel, employee, repositories, and the reason for failure would be opaque. At domain level it is clear and fast.

### Step 5: Journey 7 — Cascades (where the acceptance test shines)

```csharp
[Fact]
public void deleting_employee_removes_bookings_and_policies()
{
    // Given
    var hotelId = Given.AHotelExists("Westing");
    Given.TheHotelHasRooms(hotelId, Room.OfType("Suite").WithNumber(101));
    var employeeId = Given.AnEmployeeExists("Acme Corp");
    Given.EmployeeHasPolicy(employeeId, allowedTypes: "Suite");
    var booking = When.TheEmployeeBooks(
        employeeId, hotelId, "Suite", Today, Today.AddDays(5));
    Then.TheBookingIsConfirmed(booking);

    // When
    When.TheEmployeeIsDeleted(employeeId);

    // Then
    Then.TheEmployeeDoesNotExist(employeeId);
    Then.NoBookingsExistFor(employeeId);
    Then.NoPoliciesExistFor(employeeId);
}
```

This acceptance test verifies a journey that crosses multiple aggregates. This is where the acceptance test provides the most value: no individual unit test covers this interaction.

---

## What your repo did right (validated by this guide)

| Your decision | Why it was correct |
|---|---|
| `e04ad10` "step back to more reasonable acceptance tests" | Recognizing that the approach is not working and pivoting is the most valuable skill |
| Phase 4 (Policies): Acceptance -> Controller -> UseCase -> Repo | Genuine outside-in, commit by commit |
| Refactor/feature ratio ~1:1 | Real Red-Green-Refactor discipline |
| `1dbfdd4` "actually found a bug!!" in Hotel.SetRoom | Proof that domain tests have their own value |
| Ports & Adapters emerged from the process | Architecture is not imposed, it is discovered |
| SQL repos at the end without touching the domain | Validation that hexagonal works |

## What would have changed (validated by this guide)

| Your decision | What would have been done differently |
|---|---|
| Removing the API at the beginning to simplify | Journey 1 (add a hotel) as walking skeleton: simple as business, forces the entire pipeline |
| Acceptance tests with HTTP directly in the test | DSL with drivers that encapsulate HTTP |
| No domain tests for policy logic | Combinatorics tests at domain level |
| Large commits in Phase 5 (15 files) | One commit per step of the double loop |
| `Verify(Times.Once)` in use case tests | Fakes verifying resulting state |
| SQL persistence in Phase 6 (week 4) | Early integration test — the walking skeleton forces validating persistence from the first journey |

---

## Test Level Table

```
+-------------------+---------------------------+---------------------------+
| LEVEL             | WHAT YOU VERIFY            | TOOL                      |
+-------------------+---------------------------+---------------------------+
| Acceptance Test   | Complete business          | DSL + HttpDriver          |
|                   | journeys                  | (HTTP invisible to test)  |
+-------------------+---------------------------+---------------------------+
| Controller Test   | HTTP <-> UseCase mapping   | Use case mocks            |
|                   | (request -> command,      | You verify delegation     |
|                   |  result -> status code)   |                           |
+-------------------+---------------------------+---------------------------+
| Use Case Test     | Business orchestration    | Fakes (InMemory repos)    |
|                   | Happy + sad paths         | You verify state          |
+-------------------+---------------------------+---------------------------+
| Domain Test       | Logic with combinatorics  | No dependencies           |
|                   | (dates, policies,         | Pure logic                |
|                   |  invariants, calculations)|                           |
+-------------------+---------------------------+---------------------------+
| Integration Test  | Real persistence          | Testcontainers or         |
|                   | (SQL repos, ext. APIs)    | InMemory DB               |
+-------------------+---------------------------+---------------------------+
```

### Mock vs Fake vs Nothing Criteria

| You verify... | Tool | Example |
|---|---|---|
| That A delegated to B correctly | **Mock** | Controller -> Use Case |
| Resulting state of an operation | **Fake** | "There is 1 booking in the repo" |
| Side effect without observable state | **Mock** | "Notification was sent" |
| Pure logic without dependencies | **Nothing** | `DateRange.Overlaps()` |

---

## The Complete Flow

```
1. ACCEPTANCE TEST (RED)     Real business journey.
   |                         With DSL, business language.
   | (outer loop)            Via HttpDriver (genuine outside-in).
   |
   |                         The FIRST journey is the simplest possible
   |                         (e.g.: "add a hotel"). It acts as a
   |                         walking skeleton: it forces you to set up
   |                         the entire pipeline + DSL + drivers. It is
   |                         not a throwaway test — it is real business
   |                         that stays.
   |
   |  +--------------------------------------------------+
   |  |  INNER LOOP TDD                                  |
   |  |                                                  |
   |  |  2. Controller test -> impl (mock use case)      |
   |  |  3. Use Case test -> impl (fake repos)           |
   |  |  4. Domain test -> impl (ONLY if combinatorics)  |
   |  |  5. Integration test -> impl (real repo)         |
   |  +--------------------------------------------------+
   |
   v
6. ACCEPTANCE TEST (GREEN)   The outer loop closes.
   |
   v
7. REFACTOR                  With complete safety net.
   |
   v
8. REPEAT                    Next journey from step 1.
                             The DSL and drivers grow
                             incrementally with each journey.
```

---

## When NOT to follow this guide to the letter

- **You are exploring an unknown domain:** middle-out (starting from use case with fakes) gives you faster feedback on the logic without infrastructure noise.
- **The domain is very complex and the delivery is trivial:** inside-out (starting from domain) may be more appropriate for rule engines, algorithms, or complex state machines.
- **You are in a learning kata:** simplifying is valid. Removing the API at the beginning (as you did in `88422b7`) is a legitimate pragmatic decision to focus on what you want to practice.

Outside-in, middle-out, inside-out are **tools, not religions**. The non-negotiables are:

1. Start from the behavior
2. Your first journey should be so simple that it acts as a walking skeleton — nothing throwaway
3. Test at the appropriate level according to the complexity
4. Each step of the double loop is a commit

---

## References

- **Growing Object-Oriented Software, Guided by Tests** (Freeman & Pryce) — Canonical book on outside-in TDD with walking skeleton
- **Corporate Hotel Booking Kata** (Sandro Mancuso, Codurance) — The original kata
- **Optivem Journal** (Valentina Jemuovic) — ATDD, Use Case Driven Development, 4-layer architecture
- **Banking Kata .NET** (Valentina Cupac) — Reference example in .NET
- **Test-Driven Development by Example** (Kent Beck) — The foundations of TDD
- **Hexagonal Architecture** (Alistair Cockburn) — Original Ports & Adapters
- **Outside-In TDD** (Sandro Mancuso, Codurance) — Videos and workshops on London School
