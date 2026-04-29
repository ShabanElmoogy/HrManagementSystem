using HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class BoardTaskAttachmentErrors(IStringLocalizer<BoardTaskAttachmentRequest> localizer)
    {
        private readonly IStringLocalizer<BoardTaskAttachmentRequest> _localizer = localizer;

        public Error BoardTaskAttachmentNotFound =>
            new("BoardTaskAttachment.NotFound", _localizer[nameof(BoardTaskAttachmentNotFound)], StatusCodes.Status404NotFound);

        public Error BoardTaskAttachmentExists =>
            new("BoardTaskAttachment.Duplicated", _localizer[nameof(BoardTaskAttachmentExists)], StatusCodes.Status409Conflict);

        public Error InvalidBoardTask =>
            new("BoardTaskAttachment.InvalidTask", _localizer[nameof(InvalidBoardTask)], StatusCodes.Status400BadRequest);

        public Error InvalidFile =>
            new("BoardTaskAttachment.InvalidFile", _localizer[nameof(InvalidFile)], StatusCodes.Status400BadRequest);
    }
}
