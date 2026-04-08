# Development Report: Corporate Hotel Booking Kata

## Summary

Project developed between **May 15 and June 17, 2023** (~33 days), with **78 commits**. The goal was to implement the Corporate Hotel Booking kata following **Outside-In TDD**. The final result is a clean architecture with Ports & Adapters, domain slices (Hotels, Employees, Bookings, Policies), and dual repository implementation (InMemory + SQL).

---

## Development Phases

### Phase 1: Exploration and False Start (May 15-19, commits f7838a8..ada7f67)

**What happened:** It started with an HTTP acceptance test against a WebAPI (`HotelTest.e2e.cs`), which is the correct outside-in idea: start from the outside. However, it was quickly decided to **remove the API layer** to simplify (`88422b7: chore: remove API implementation to simplify kata`).

The work shifted to working directly with `HotelService`, creating unit tests with mocks (`AddHotel_ShouldAddHotelToRepository`) and exception tests (`should_throw_exception_when_hotel_already_exists`).

**Outside-In Pattern:** Partially followed. It started from the outside (HTTP), but was abandoned quickly. The red/green tests are well labeled in the commits (`test (red)`, `feat (green)`), which shows TDD discipline.

**Key moment:** The commit `e04ad10` ("step back to more reasonable acceptance tests") reveals an important decision: the previous tests were discarded and a new service-level (not HTTP) acceptance test was created, defining the complete booking user journey from start to finish.

### Phase 2: Building the Core with Classic TDD (May 20-25, commits e04ad10..a0d199b)

**What happened:** With the acceptance test as a guide, the domain was built slice by slice:

1. **HotelService** - AddHotel + SetRoom (with unit tests + InMemory repository integration)
2. **EmployeeService** - AddEmployee
3. **BookingService** - Book

Multiple structural refactors were applied in this phase: separation by slices (`Hotels/`, `Employees/`, `Bookings/`), extraction of the `Hotel` entity, encapsulation, and improvement of the repositories to follow Ports & Adapters.

**Outside-In Pattern:** In this phase it was more **middle-out/classicist TDD**. The acceptance test existed but development was guided more by service unit tests and repository integration tests than by the failing acceptance test.

**Notable finding:** Many refactor commits in proportion to feature commits - indicates good discipline of "taking small steps" and constantly cleaning up.

### Phase 3: Introduction of Value Objects and Structural Refactoring (May 26-29, commits 5500190..6d7760d)

**What happened:** 

- **Value Objects**: Primitive IDs (int) were changed to Value Objects (`HotelId`, `EmployeeId`, `CompanyId`, `BookingId`), first as classes and then as `record`.
- **Use Cases**: Services were split into individual use cases (`AddHotelUseCase`, `SetRoomUseCase`, `FindHotelUseCase`, etc.)
- **API Layer**: The distribution layer was reintroduced with controllers and HTTP acceptance tests via `WebApplicationFactory`.
- The old acceptance tests (at service level) were removed in favor of the new ones via HTTP.

**Outside-In Pattern:** The reintroduction of the API was key. The commit `bc8b045` ("add API distribution layer with acceptance test failing because not implemented") is **textbook outside-in**: write the HTTP acceptance test first, watch it fail, and implement the controllers.

### Phase 4: Complete Feature with Clear Outside-In - Policies (May 30 - June 6, commits 1dbfdd4..39870af)

**This was the phase where Outside-In TDD was best applied.** The sequence for Employee Policies was:

1. `56f364c` - **Acceptance test (User Journey)**: E2E test describing the complete scenario via HTTP
2. `73332c5` - **Controller test**: Unit test for `PoliciesController`
3. `ada0201` - **Controller implementation**: Minimal controller implementation
4. `6326bea` - **Use Case test**: Unit test for `AddEmployeePolicyUseCase`
5. `2e296e4` - **Repository integration test**: Test for `InMemoryPoliciesRepository`
6. `51297ff` - **Repository implementation**: Repository implementation

Then for the business rule "booking not allowed by policy":

7. `d303b74` - **Controller test**: `BookRoom_WhenEmployeeBookingPolicyException_ShouldReturnConflict`
8. `df6e820` - **Use Case test**: `AddBooking_WhenPolicyIsNotContained_ShouldThrowEmployeePolicyException`
9. `0077d33` - **Use Case implementation**
10. `0a24aff` - **Repository** `GetEmployeePolicy` implementation

**This is the canonical Outside-In pattern:** Acceptance -> Controller -> UseCase -> Repository, from the outside in.

### Phase 5: Completing Business Rules (June 7-12, commits e3dd439..bf49e61)

**What happened:** The remaining functionalities were added:

- Company policies
- Available room type validation in hotel
- Room availability validation
- Cascade deletion of employee (policies + bookings)
- Invalid booking period validation

Major structural refactoring: separation of the project into multiple assemblies (`CorporateHotelBooking.Api`, `CorporateHotelBooking.Data.InMemory`, `CorporateHotelBooking.Application`). Renaming of E2E tests from "data setup" to "user journey".

**Outside-In Pattern:** The pattern was maintained but with larger commits (less granular). The commit `e3dd439` for example includes 15 files - mixing test + implementation in a single commit.

### Phase 6: SQL Persistence (June 13-17, commits 204bf8c..2a6f4bf)

**What happened:** Real SQL repositories were implemented with Entity Framework:

- First with InMemory SQL (EF InMemory provider)
- Then with real LocalDB
- Finally with **TestContainers** (MsSQL in Docker)

Data models, mappers, and test fixtures were created for the three persistence strategies.

**Outside-In Pattern:** Does not apply directly - this is an **infrastructure** phase guided by the already defined ports (interfaces). The integration tests for each SQL repository were created one by one, which shows TDD discipline at the integration level.

---

## Statistics

| Metric | Value |
|---|---|
| Total commits | 78 |
| `test` commits (red) | 12 |
| `feat` commits (green) | ~30 |
| `refactor` commits | ~30 |
| `chore` commits | ~6 |
| Refactor/feature ratio | ~1:1 |
| Duration | 33 days |

---

## Conclusions

### 1. Outside-In was achieved, but not from the beginning

The first phases (1-2) were more **classicist TDD / middle-out**: service unit tests were written and implemented. The true Outside-In arrived in **Phase 4 (Policies)**, where the Acceptance -> Controller -> UseCase -> Repository sequence was followed clearly and with discipline.

### 2. The "step back" was the most important decision

The commit `e04ad10` ("step back to more reasonable acceptance tests") is the project's turning point. Recognizing that the HTTP approach was too complex at the beginning and pivoting to service-level acceptance tests was pragmatic. Later, when the architecture matured, the HTTP layer was reintroduced naturally.

### 3. Excellent refactoring discipline

The ~1:1 ratio between refactor and feature commits indicates that the Red-Green-**Refactor** cycle was respected. The refactors were not cosmetic - they included important architectural decisions: Value Objects, Use Cases, Ports & Adapters, separation into assemblies.

### 4. The commits tell the story

The `test (red)`, `feat (green)`, `refactor` prefixes in the early commits are very useful for understanding the TDD flow. This practice relaxed in later commits, where features and tests were mixed in a single commit.

### 5. A real bug was found thanks to the tests

The commit `1dbfdd4` ("actually found a bug!!") demonstrates the practical value of tests: when adding unit tests for `Hotel.SetRoom`, a bug was discovered that had not been detected by higher-level tests.

### 6. Natural architectural evolution

The final architecture (Ports & Adapters with domain slices, use cases, and dual repository implementation) was not designed from the start but rather **emerged** from the TDD + refactoring process:

```
Phase 1: Monolithic HotelService
Phase 2: Domain slices (Hotels/, Employees/, Bookings/)
Phase 3: Value Objects + Use Cases (SRP)
Phase 4: Policies as a new slice, pure Outside-In
Phase 5: Separate assemblies (Api, Application, Data.InMemory, Data.Sql)
```

### 7. Persistence as an implementation detail

Phase 6 validates that Ports & Adapters worked: SQL could be added without touching a single line of domain code. The InMemory repositories served as the first implementation and the SQL ones were added later implementing the same interfaces.

### 8. Area for improvement: granularity in intermediate phases

In Phase 5 the commits became larger and less atomic. Maintaining the discipline of small commits (as in Phases 1-4) would have given more visibility into the process.

---

## Outside-In Flow Diagram (as it ended up in Phase 4)

```
[HTTP Acceptance Test]         <-- E2E User Journey
        |
[Controller Test]              <-- UseCase Mock
        |
[UseCase Test]                 <-- Repository Mock
        |
[InMemory Repository Test]    <-- Integration
        |
[Implementation]               <-- Green at all levels
```

This flow was consistently maintained for the Policies features and is the recommended pattern for continuing development.
