namespace Company.Nacha.NachaElements.Enum
{
	public enum ReturnReasonCode
	{
		Insufficientfunds = 	1,
		Accountclosed	=2,
		Noaccount=	3,
		InvalidAccountNumber =	4,
		ReturnedOdfiRequest	=6,
		AuthorizationRevokedByCustomer	=7,
		PaymentStopped	=8,
		UncollectedFunds	=9,
		CustomerAdvicesNotAuthorized	=10,
		ReceivingDfiNotQualifiedParticipant	=13,
		RepPayeeDeceasedOrUnableToContinue	=14,
		BeneficiaryOrAccountHolderDeceased	=15,
		AccountFrozen	=16,
		FileRecordEditCriteria	=17,
		NonTransactionAccount	=20,
		CreditEntryRefusedByReceiver=	23,
	}
}