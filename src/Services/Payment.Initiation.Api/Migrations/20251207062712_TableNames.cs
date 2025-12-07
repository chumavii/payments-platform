using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payment.Initiation.Api.Migrations
{
    /// <inheritdoc />
    public partial class TableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxStates",
                table: "OutboxStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_InboxStates_MessageId_ConsumerId",
                table: "InboxStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxStates",
                table: "InboxStates");

            migrationBuilder.RenameTable(
                name: "OutboxStates",
                newName: "outbox_states");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                newName: "outbox_messages");

            migrationBuilder.RenameTable(
                name: "InboxStates",
                newName: "inbox_states");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxStates_Created",
                table: "outbox_states",
                newName: "IX_outbox_states_Created");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessages_OutboxId_SequenceNumber",
                table: "outbox_messages",
                newName: "IX_outbox_messages_OutboxId_SequenceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessages_InboxMessageId_InboxConsumerId_SequenceNumber",
                table: "outbox_messages",
                newName: "IX_outbox_messages_InboxMessageId_InboxConsumerId_SequenceNumb~");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessages_ExpirationTime",
                table: "outbox_messages",
                newName: "IX_outbox_messages_ExpirationTime");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessages_EnqueueTime",
                table: "outbox_messages",
                newName: "IX_outbox_messages_EnqueueTime");

            migrationBuilder.RenameIndex(
                name: "IX_InboxStates_Delivered",
                table: "inbox_states",
                newName: "IX_inbox_states_Delivered");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outbox_states",
                table: "outbox_states",
                column: "OutboxId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outbox_messages",
                table: "outbox_messages",
                column: "SequenceNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_inbox_states_MessageId_ConsumerId",
                table: "inbox_states",
                columns: new[] { "MessageId", "ConsumerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_inbox_states",
                table: "inbox_states",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outbox_states",
                table: "outbox_states");

            migrationBuilder.DropPrimaryKey(
                name: "PK_outbox_messages",
                table: "outbox_messages");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_inbox_states_MessageId_ConsumerId",
                table: "inbox_states");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inbox_states",
                table: "inbox_states");

            migrationBuilder.RenameTable(
                name: "outbox_states",
                newName: "OutboxStates");

            migrationBuilder.RenameTable(
                name: "outbox_messages",
                newName: "OutboxMessages");

            migrationBuilder.RenameTable(
                name: "inbox_states",
                newName: "InboxStates");

            migrationBuilder.RenameIndex(
                name: "IX_outbox_states_Created",
                table: "OutboxStates",
                newName: "IX_OutboxStates_Created");

            migrationBuilder.RenameIndex(
                name: "IX_outbox_messages_OutboxId_SequenceNumber",
                table: "OutboxMessages",
                newName: "IX_OutboxMessages_OutboxId_SequenceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_outbox_messages_InboxMessageId_InboxConsumerId_SequenceNumb~",
                table: "OutboxMessages",
                newName: "IX_OutboxMessages_InboxMessageId_InboxConsumerId_SequenceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_outbox_messages_ExpirationTime",
                table: "OutboxMessages",
                newName: "IX_OutboxMessages_ExpirationTime");

            migrationBuilder.RenameIndex(
                name: "IX_outbox_messages_EnqueueTime",
                table: "OutboxMessages",
                newName: "IX_OutboxMessages_EnqueueTime");

            migrationBuilder.RenameIndex(
                name: "IX_inbox_states_Delivered",
                table: "InboxStates",
                newName: "IX_InboxStates_Delivered");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxStates",
                table: "OutboxStates",
                column: "OutboxId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages",
                column: "SequenceNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_InboxStates_MessageId_ConsumerId",
                table: "InboxStates",
                columns: new[] { "MessageId", "ConsumerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxStates",
                table: "InboxStates",
                column: "Id");
        }
    }
}
