USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomApp_RegClaimInfo_MotorAccidentParties]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CustomApp_RegClaimInfo_MotorAccidentParties]
	@IncidentId UNIQUEIDENTIFIER 
AS 
    BEGIN		
        BEGIN TRANSACTION 
			--DECLARE @TicketNumber AS NVARCHAR(20) = 'CAS201702-00003';
			--DECLARE @IncidentId UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874'
			select top 1 [Case].IncidentId
				, [MotorAccidentParties].pfc_event_code
				, [MotorAccidentParties].pfc_event_sequence
				, [MotorAccidentParties].pfc_parties_sequence
				/***[accidentPartyInfo]***/
				, [MotorAccidentParties].pfc_parties_fullname as [accidentPartyFullname]
				, [MotorAccidentParties].pfc_phoneno as [accidentPartyPhone]
				, [MotorAccidentParties].pfc_licence_no as [accidentPartyCarPlateNumber]
				, [MotorAccidentParties].pfc_brand as [accidentPartyCarModel]
				, 'N' as [accidentPartyInsuredFlag]--[MotorAccidentParties].pfc_accident_party_insured_flag as [accidentPartyInsuredFlag]
				, [MotorAccidentParties].pfc_insurance_name as [accidentPartyInsuranceCompany]
				, [MotorAccidentParties].pfc_policy_type as [accidentPartyPolicyType]
				, [MotorAccidentParties].pfc_policyno as [accidentPartyPolicyNumber]
				, null as [accidentPartyPolicyExpdate]--[MotorAccidentParties].pfc_accident_party_policy_expdate as [accidentPartyPolicyExpdate]
				, null as [demageOfPartyCar]--[MotorAccidentParties].pfc_demage_of_party_car as [demageOfPartyCar]
			from dbo.IncidentBase [Case] with (nolock)
			inner join dbo.pfc_motor_accidentBase [MotorAccident] with (nolock)
				on [MotorAccident].pfc_parent_caseId = [Case].IncidentId
			inner join dbo.pfc_motor_accident_partiesBase [MotorAccidentParties] with (nolock)
				on [MotorAccidentParties].pfc_parent_motor_accidentId = [MotorAccident].pfc_motor_accidentId
			where [Case].IncidentId = @IncidentId

		COMMIT TRANSACTION -- Commit transaction if the transaction is successful
    END

/*
DECLARE @ticketNo AS NVARCHAR(20) = 'CAS201702-00003';
DECLARE @uniqueID AS UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
EXEC [dbo].[sp_CustomApp_RegClaimInfo_MotorAccidentParties] @uniqueID, @ticketNo


DECLARE @IncidentId AS UNIQUEIDENTIFIER = 'D246086E-C1EE-E611-80D4-0050568D1874';
EXEC [dbo].[sp_CustomApp_RegClaimInfo_MotorAccidentParties] @IncidentId

*/
GO
