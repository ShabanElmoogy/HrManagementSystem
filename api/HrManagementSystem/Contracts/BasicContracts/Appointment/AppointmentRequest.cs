namespace HrManagementSystem.Contracts.BasicContracts.Appointment;

public record AppointmentRequest
(
     int Id,
     DateTime Start,
     DateTime End,
     string Text
);
