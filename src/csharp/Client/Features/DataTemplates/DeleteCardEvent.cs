using DewIt.Model.DataTypes;
using DewIt.Model.Events;

namespace DewIt.Client.Features.DataTemplates;

public class DeleteCardEvent : PubSubEvent<Card> { }