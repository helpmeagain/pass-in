﻿using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register;
public class RegisterEventUseCase
{
    public ResponseRegisteredJson Execute(RequestEventJson request)
    {
        Validate(request);

        var dbContext = new PassInDbContext();
        var entity = new Infrastructure.Entities.Event
        {
            Title = request.Title,
            Details = request.Details,
            Maximum_Attendees = request.MaximumAttendees,
            Slug = request.Title.ToLower().Replace(" ", "-")
        };

        dbContext.Events.Add(entity);
        dbContext.SaveChanges();

        return new ResponseRegisteredJson { Id = entity.Id };
    }

    private void Validate(RequestEventJson request)
    {
        if (request.MaximumAttendees < 1)
            throw new OnValidationException("Maximum attendees must be greater than 0.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new OnValidationException("Name is required.");

        if (string.IsNullOrWhiteSpace(request.Details))
            throw new OnValidationException("Name is required.");
    }
}
