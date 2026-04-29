namespace HrManagementSystem.Contracts.BasicContracts.Appointment;

public record AppointmentResponse
(
     int? Id,
     DateTime Start,
     DateTime End,
     string Text
);
