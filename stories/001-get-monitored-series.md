# Story 001 — GET /api/series/monitored

## Summary

Add an endpoint that returns only the series the user is currently monitoring.
This is the primary list view — most users will have this open by default.

## Acceptance Criteria

- `GET /api/series/monitored` returns `200 OK` with an array of `SeriesDto`
- Only series where `Monitored == true` are returned
- Returns an empty array (not 404) if no series are monitored
- Response shape matches the existing `SeriesDto`

## Size

1-point

## Notes

- Filtering logic belongs in the service layer, not the controller (see POLICIES.md P01)
- Follow the pattern in `SeriesController.GetAll()` and `SeriesService.GetAllAsync()`
- New service method: `GetMonitoredAsync()` on `ISeriesService`
