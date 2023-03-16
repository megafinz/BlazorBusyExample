This is an example of how you can manage busy states across the app.

## Busy Service

`BusyService` is a singleton that stores `BusyState` objects.

## Busy State

These objects can be referred to by a string key through the `BusyService` so you can easily retrieve the same object in different parts of the application:
- They count activations and deactivations so they can be used concurrently. 
- They can notify interested parties when their state changes.

## Busy Provider

This is a UI-component that wraps around the specific `BusyState` object and provides  a simplified way of watching for busy state change notifications to other UI components.
