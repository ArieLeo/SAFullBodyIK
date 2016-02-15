﻿// Copyright (c) 2016 Nora
// Released under the MIT license
// http://opensource.org/licenses/mit-license.phpusing

#if SAFULLBODYIK_DEBUG
#define SAFULLBODYIK_DEBUG_CONSTRUCT_TIME
#endif

using UnityEngine;
using System.Collections.Generic;

namespace SA
{
	[System.Serializable]
	public partial class FullBodyIK
	{
		[System.Serializable]
		public class BodyBones
		{
			public Bone hips;
			public Bone spine;
			public Bone spine2;
		}

		[System.Serializable]
		public class HeadBones
		{
			public Bone neck;
			public Bone head;
			public Bone leftEye;
			public Bone rightEye;
		}

		[System.Serializable]
		public class LegBones
		{
			public Bone leg;
			public Bone knee;
			public Bone foot;
		}

		[System.Serializable]
		public class ArmBones
		{
			public Bone shoulder;
			public Bone arm;
			public Bone[] armRoll;
			public Bone elbow;
			public Bone[] elbowRoll;
			public Bone wrist;

			public void Repair()
			{
				SafeResize( ref armRoll, MaxArmRollLength );
				SafeResize( ref elbowRoll, MaxElbowRollLength );
			}
		}

		[System.Serializable]
		public class FingersBones
		{
			public Bone[] thumb;
			public Bone[] index;
			public Bone[] middle;
			public Bone[] ring;
			public Bone[] little;

			public void Repair()
			{
				SafeResize( ref thumb, MaxHandFingerLength );
				SafeResize( ref index, MaxHandFingerLength );
				SafeResize( ref middle, MaxHandFingerLength );
				SafeResize( ref ring, MaxHandFingerLength );
				SafeResize( ref little, MaxHandFingerLength );
				// Memo: Don't alloc each bone instances.( Alloc in _Prefix() ).
			}
		}

		[System.Serializable]
		public class BodyEffectors
		{
			public Effector hips;
		}

		[System.Serializable]
		public class HeadEffectors
		{
			public Effector neck;
			public Effector head;
			public Effector eyes;
		}

		[System.Serializable]
		public class LegEffectors
		{
			public Effector knee;
			public Effector foot;
		}

		[System.Serializable]
		public class ArmEffectors
		{
			public Effector arm;
			public Effector elbow;
			public Effector wrist;
		}

		[System.Serializable]
		public class FingersEffectors
		{
			public Effector thumb;
			public Effector index;
			public Effector middle;
			public Effector ring;
			public Effector little;
		}

		public enum AutomaticBool
		{
			Auto = -1,
			Disable = 0,
			Enable = 1,
		}

		[System.Serializable]
		public class Settings
		{
			public AutomaticBool animatorEnabled = AutomaticBool.Auto;
			public AutomaticBool resetTransforms = AutomaticBool.Auto;

			public bool automaticPrepareHumanoid = true;

			public bool automaticConfigureRollEnabled = false;
			public bool rollEnabled = false;

			public bool createEffectorTransform = true;

			public ModelTemplate modelTemplate = ModelTemplate.Standard;

			[System.Serializable]
			public class BodyIK
			{
				public bool lowerSolveEnabled = true;
				public bool upperSolveEnabled = true;
				public bool computeWorldTransform = true;

				public float shoulderLimitAngleYPlus = 30.0f;
				public float shoulderLimitAngleYMinus = 1.0f;
				public float shoulderLimitAngleZ = 30.0f;

				public float spineDirXLegToArmRate = 0.5f;
				public float spineDirYLerpRate = 0.7f;

				public float upperPreTranslateRate = 0.2f;
				public float upperCenterLegRotateRate = 0.673f;
				public float upperSpineRotateRate = 0.775f;
				public float upperPostTranslateRate = 1.0f;

				public bool upperSolveSpineEnabled = true;
				public bool upperSolveSpine2Enabled = true;

				public float upperCenterLegLerpRate = 1.0f;
				public float upperSpineLerpRate = 1.0f;

				public float spineLimitAngleX = 40.0f;
				public float spineLimitAngleY = 10.0f;

				public float upperContinuousPreTranslateRate = 0.2f;
				public float upperContinuousPreTranslateStableRate = 0.65f;
				public float upperContinuousCenterLegRotationStableRate = 0.0f;
				public float upperContinuousPostTranslateStableRate = 0.01f;

				public float upperNeckToCenterLegRate = 0.6f;
				public float upperNeckToSpineRate = 0.9f;
				public float upperEyesToCenterLegRate = 0.2f;
				public float upperEyesToSpineRate = 0.5f;
				public float upperEyesRateYUp = 0.25f;
				public float upperEyesRateYDown = 0.5f;
				public float upperEyesLimitAngleX = 35.0f;
				public float upperEyesLimitAngleYUp = 10.0f;
				public float upperEyesLimitAngleYDown = 45.0f;
				public float upperEyesBackOffsetZ = 0.5f; // Lock when behind looking.
			}

			[System.Serializable]
			public class LimbIK
			{
				public float automaticKneeBaseAngle = 0.0f;

				public bool presolveKneeEnabled = true;
				public bool presolveElbowEnabled = true;
				public float presolveKneeRate = 1.0f;
				public float presolveKneeLerpAngle = 10.0f;
				public float presolveKneeLerpLengthRate = 0.1f;
				public float presolveElbowRate = 1.0f;
				public float presolveElbowLerpAngle = 10.0f;
				public float presolveElbowLerpLengthRate = 0.1f;

				public bool prefixLegEffectorEnabled = true;

				public float prefixLegUpperLimitAngle = 60.0f;
				public float prefixKneeUpperLimitAngle = 45.0f;

				public float legEffectorMinLengthRate = 0.1f;
				public float legEffectorMaxLengthRate = 0.9999f;
				public float armEffectorMaxLengthRate = 0.9999f;

				public bool armBasisForcefixEnabled = true;
				public float armBasisForcefixEffectorLengthRate = 0.99f;
				public float armBasisForcefixEffectorLengthLerpRate = 0.03f;

				// Arm back area.(Automatic only, Based on localXZ)
				public float armEffectorBackBeginAngle = 5.0f;
				public float armEffectorBackCoreBeginAngle = -10.0f;
				public float armEffectorBackCoreEndAngle = -30.0f;
				public float armEffectorBackEndAngle = -160.0f;

				// Arm back area.(Automatic only, Based on localYZ)
				public float armEffectorBackCoreUpperAngle = 8.0f;
				public float armEffectorBackCoreLowerAngle = -15.0f;

				// Arm elbow angles.(Automatic only)
				public float automaticElbowBaseAngle = 30.0f;
				public float automaticElbowLowerAngle = 90.0f;
				public float automaticElbowUpperAngle = 90.0f;
				public float automaticElbowBackUpperAngle = 180.0f;
				public float automaticElbowBackLowerAngle = 330.0f;

				// Arm elbow limit angles.(Automatic / Manual)
				public float elbowFrontInnerLimitAngle = 5.0f;
				public float elbowBackInnerLimitAngle = 0.0f;
			}

			[System.Serializable]
			public class HeadIK
			{
			}

			[System.Serializable]
			public class FingerIK
			{
			}

			public BodyIK bodyIK;
			public LimbIK limbIK;
			public HeadIK headIK;
			public FingerIK fingerIK;

			public void Prefix()
			{
				SafeNew( ref bodyIK );
				SafeNew( ref limbIK );
				SafeNew( ref headIK );
				SafeNew( ref fingerIK );
			}
		}

		[System.Serializable]
		public class EditorSettings
		{
			public bool isAdvanced;
			public int toolbarSelected;
			public bool isShowEffectorTransform;
		}

		// Memo: Not Serializable
		public class InternalValues
		{
			public bool animatorEnabled;
			public bool resetTransforms;
			public bool continuousSolverEnabled;

			public Vector3 defaultRootPosition = Vector3.zero;
			public Matrix3x3 defaultRootBasis = Matrix3x3.identity;
			public Matrix3x3 defaultRootBasisInv = Matrix3x3.identity;
			public Quaternion defaultRootRotation = Quaternion.identity;

#if SAFULLBODYIK_DEBUG
			public DebugData debugData = new DebugData();
#endif

			[System.Diagnostics.Conditional( "SAFULLBODYIK_DEBUG" )]
			public void ClearDegugPoints()
			{
#if SAFULLBODYIK_DEBUG
				debugData.debugPoints.Clear();
#endif
			}

			[System.Diagnostics.Conditional( "SAFULLBODYIK_DEBUG" )]
			public void AddDebugPoint( Vector3 pos )
			{
#if SAFULLBODYIK_DEBUG
				debugData.debugPoints.Add( new DebugPoint( pos ) );
#endif
			}

			[System.Diagnostics.Conditional( "SAFULLBODYIK_DEBUG" )]
			public void AddDebugPoint( Vector3 pos, Color color )
			{
#if SAFULLBODYIK_DEBUG
				debugData.debugPoints.Add( new DebugPoint( pos, color ) );
#endif
			}

			[System.Diagnostics.Conditional( "SAFULLBODYIK_DEBUG" )]
			public void AddDebugPoint( Vector3 pos, Color color, float size )
			{
#if SAFULLBODYIK_DEBUG
				debugData.debugPoints.Add( new DebugPoint( pos, color, size ) );
#endif
			}

			[System.Diagnostics.Conditional( "SAFULLBODYIK_DEBUG" )]
			public void UpdateDebugValue( string name, ref int v )
			{
#if SAFULLBODYIK_DEBUG
				debugData.UpdateValue( name, ref v );
#endif
			}

			[System.Diagnostics.Conditional( "SAFULLBODYIK_DEBUG" )]
			public void UpdateDebugValue( string name, ref float v )
			{
#if SAFULLBODYIK_DEBUG
				debugData.UpdateValue( name, ref v );
#endif
			}

			[System.Diagnostics.Conditional( "SAFULLBODYIK_DEBUG" )]
			public void UpdateDebugValue( string name, ref bool v )
			{
#if SAFULLBODYIK_DEBUG
				debugData.UpdateValue( name, ref v );
#endif
			}

			public class BodyIK
			{
				public CachedDegreesToSin shoulderLimitThetaYPlus = CachedDegreesToSin.zero;
				public CachedDegreesToSin shoulderLimitThetaYMinus = CachedDegreesToSin.zero;
				public CachedDegreesToSin shoulderLimitThetaZ = CachedDegreesToSin.zero;

				public CachedRate01 upperPreTranslateRate = CachedRate01.zero;
				public CachedRate01 upperPostTranslateRate = CachedRate01.zero;

				public CachedRate01 upperCenterLegRotateRate = CachedRate01.zero;
				public CachedRate01 upperSpineRotateRate = CachedRate01.zero;
				public bool isFuzzyUpperCenterLegAndSpineRotationRate = true;

				public CachedDegreesToSin upperEyesLimitThetaX = CachedDegreesToSin.zero;
				public CachedDegreesToSin upperEyesLimitThetaYUp = CachedDegreesToSin.zero;
				public CachedDegreesToSin upperEyesLimitThetaYDown = CachedDegreesToSin.zero;

				public CachedScaledValue spineLimitAngleX = CachedScaledValue.zero; // Mathf.Deg2Rad(Not sin)
				public CachedScaledValue spineLimitAngleY = CachedScaledValue.zero; // Mathf.Deg2Rad(Not sin)

				public CachedRate01 upperContinuousPreTranslateRate = CachedRate01.zero;
				public CachedRate01 upperContinuousPreTranslateStableRate = CachedRate01.zero;
				public CachedRate01 upperContinuousCenterLegRotationStableRate = CachedRate01.zero;
				public CachedRate01 upperContinuousPostTranslateStableRate = CachedRate01.zero;

				public void Update( Settings.BodyIK settingsBodyIK )
				{
					// Optimize: Reduce C# fuction call.
					Assert( settingsBodyIK != null );

					if( shoulderLimitThetaYPlus._degrees != settingsBodyIK.shoulderLimitAngleYPlus ) {
						shoulderLimitThetaYPlus._Reset( settingsBodyIK.shoulderLimitAngleYPlus );
					}
					if( shoulderLimitThetaYMinus._degrees != settingsBodyIK.shoulderLimitAngleYMinus ) {
						shoulderLimitThetaYMinus._Reset( settingsBodyIK.shoulderLimitAngleYMinus );
					}
					if( shoulderLimitThetaZ._degrees != settingsBodyIK.shoulderLimitAngleZ ) {
						shoulderLimitThetaZ._Reset( settingsBodyIK.shoulderLimitAngleZ );
					}

					if( upperPreTranslateRate._value != settingsBodyIK.upperPreTranslateRate ) {
						upperPreTranslateRate._Reset( settingsBodyIK.upperPreTranslateRate );
					}
					if( upperPostTranslateRate._value != settingsBodyIK.upperPreTranslateRate ) {
						upperPostTranslateRate._Reset( settingsBodyIK.upperPreTranslateRate );
					}

					if( upperCenterLegRotateRate._value != settingsBodyIK.upperCenterLegRotateRate ||
						upperSpineRotateRate._value != settingsBodyIK.upperSpineRotateRate ) {
						upperCenterLegRotateRate._Reset( settingsBodyIK.upperCenterLegRotateRate );
						upperSpineRotateRate._Reset( Mathf.Max( settingsBodyIK.upperCenterLegRotateRate, settingsBodyIK.upperSpineRotateRate ) );
						isFuzzyUpperCenterLegAndSpineRotationRate = IsFuzzy( upperCenterLegRotateRate.value, upperSpineRotateRate.value );
					}

					if( upperEyesLimitThetaX._degrees != settingsBodyIK.upperEyesLimitAngleX ) {
						upperEyesLimitThetaX._Reset( settingsBodyIK.upperEyesLimitAngleX );
					}
					if( upperEyesLimitThetaYUp._degrees != settingsBodyIK.upperEyesLimitAngleYUp ) {
						upperEyesLimitThetaYUp._Reset( settingsBodyIK.upperEyesLimitAngleYUp );
					}
					if( upperEyesLimitThetaYDown._degrees != settingsBodyIK.upperEyesLimitAngleYDown ) {
						upperEyesLimitThetaYDown._Reset( settingsBodyIK.upperEyesLimitAngleYDown );
					}

					if( spineLimitAngleX._a != settingsBodyIK.spineLimitAngleX ) {
						spineLimitAngleX._Reset( settingsBodyIK.spineLimitAngleX, Mathf.Deg2Rad );
					}
					if( spineLimitAngleY._a != settingsBodyIK.spineLimitAngleY ) {
						spineLimitAngleY._Reset( settingsBodyIK.spineLimitAngleY, Mathf.Deg2Rad );
					}

					if( upperContinuousPreTranslateRate._value != settingsBodyIK.upperContinuousPreTranslateRate ) {
						upperContinuousPreTranslateRate._Reset( settingsBodyIK.upperContinuousPreTranslateRate );
					}
					if( upperContinuousPreTranslateStableRate._value != settingsBodyIK.upperContinuousPreTranslateStableRate ) {
						upperContinuousPreTranslateStableRate._Reset( settingsBodyIK.upperContinuousPreTranslateStableRate );
					}
					if( upperContinuousCenterLegRotationStableRate._value != settingsBodyIK.upperContinuousCenterLegRotationStableRate ) {
						upperContinuousCenterLegRotationStableRate._Reset( settingsBodyIK.upperContinuousCenterLegRotationStableRate );
					}
					if( upperContinuousPostTranslateStableRate._value != settingsBodyIK.upperContinuousPostTranslateStableRate ) {
						upperContinuousPostTranslateStableRate._Reset( settingsBodyIK.upperContinuousPostTranslateStableRate );
					}
				}
			}

			public class LimbIK
			{
				public CachedDegreesToSin armEffectorBackBeginTheta = CachedDegreesToSin.zero;
				public CachedDegreesToSin armEffectorBackCoreBeginTheta = CachedDegreesToSin.zero;
				public CachedDegreesToCos armEffectorBackCoreEndTheta = CachedDegreesToCos.zero;
				public CachedDegreesToCos armEffectorBackEndTheta = CachedDegreesToCos.zero;

				public CachedDegreesToSin armEffectorBackCoreUpperTheta = CachedDegreesToSin.zero;
				public CachedDegreesToSin armEffectorBackCoreLowerTheta = CachedDegreesToSin.zero;

				public CachedDegreesToSin elbowFrontInnerLimitTheta = CachedDegreesToSin.zero;
				public CachedDegreesToSin elbowBackInnerLimitTheta = CachedDegreesToSin.zero;

				public void Update( Settings.LimbIK settingsLimbIK )
				{
					// Optimize: Reduce C# fuction call.
					Assert( settingsLimbIK != null );

					if( armEffectorBackBeginTheta._degrees != settingsLimbIK.armEffectorBackBeginAngle ) {
						armEffectorBackBeginTheta._Reset( settingsLimbIK.armEffectorBackBeginAngle );
					}
					if( armEffectorBackCoreBeginTheta._degrees != settingsLimbIK.armEffectorBackCoreBeginAngle ) {
						armEffectorBackCoreBeginTheta._Reset( settingsLimbIK.armEffectorBackCoreBeginAngle );
					}
					if( armEffectorBackCoreEndTheta._degrees != settingsLimbIK.armEffectorBackCoreEndAngle ) {
						armEffectorBackCoreEndTheta._Reset( settingsLimbIK.armEffectorBackCoreEndAngle );
					}
					if( armEffectorBackEndTheta._degrees != settingsLimbIK.armEffectorBackEndAngle ) {
						armEffectorBackEndTheta._Reset( settingsLimbIK.armEffectorBackEndAngle );
					}

					if( armEffectorBackCoreUpperTheta._degrees != settingsLimbIK.armEffectorBackCoreUpperAngle ) {
						armEffectorBackCoreUpperTheta._Reset( settingsLimbIK.armEffectorBackCoreUpperAngle );
					}
					if( armEffectorBackCoreLowerTheta._degrees != settingsLimbIK.armEffectorBackCoreLowerAngle ) {
						armEffectorBackCoreLowerTheta._Reset( settingsLimbIK.armEffectorBackCoreLowerAngle );
					}

					if( elbowFrontInnerLimitTheta._degrees != settingsLimbIK.elbowFrontInnerLimitAngle ) {
						elbowFrontInnerLimitTheta._Reset( settingsLimbIK.elbowFrontInnerLimitAngle );
					}
					if( elbowBackInnerLimitTheta._degrees != settingsLimbIK.elbowBackInnerLimitAngle ) {
						elbowBackInnerLimitTheta._Reset( settingsLimbIK.elbowBackInnerLimitAngle );
					}
				}
			}

			public LimbIK limbIK = new LimbIK();
			public BodyIK bodyIK = new BodyIK();
		}

		public Transform rootTransform;

		[System.NonSerialized]
		public InternalValues internalValues = new InternalValues();

		public Settings settings;
		public EditorSettings editorSettings;

		public BodyBones bodyBones;
		public HeadBones headBones;
		public LegBones leftLegBones;
		public LegBones rightLegBones;
		public ArmBones leftArmBones;
		public ArmBones rightArmBones;
		public FingersBones leftHandFingersBones;
		public FingersBones rightHandFingersBones;

		public Effector rootEffector;
		public BodyEffectors bodyEffectors;
		public HeadEffectors headEffectors;
		public LegEffectors leftLegEffectors;
		public LegEffectors rightLegEffectors;
		public ArmEffectors leftArmEffectors;
		public ArmEffectors rightArmEffectors;
		public FingersEffectors leftHandFingersEffectors;
		public FingersEffectors rightHandFingersEffectors;

		public Bone[] bones { get { return _bones; } }
		public Effector[] effectors { get { return _effectors; } }

		Bone[] _bones = new Bone[(int)BoneType.Max];
		Effector[] _effectors = new Effector[(int)EffectorLocation.Max];

		BodyIK _bodyIK;
		LimbIK[] _limbIK = new LimbIK[(int)LimbIKLocation.Max];
		HeadIK _headIK;
		FingerIK[] _fingerIK = new FingerIK[(int)FingerIKType.Max];

		bool _isPrefixed;
		[SerializeField]
		bool _isPrefixedAtLeastOnce;

		public void Initialize( Transform rootTransorm_ )
		{
			if( rootTransform != rootTransorm_ ) {
				rootTransform = rootTransorm_;
			}

#if SAFULLBODYIK_DEBUG_CONSTRUCT_TIME
			float constructBeginTime = Time.realtimeSinceStartup;
#endif
			_Prefix();
#if SAFULLBODYIK_DEBUG_CONSTRUCT_TIME
			float prefixEndTime = Time.realtimeSinceStartup;
#endif
			ConfigureBoneTransforms();
#if SAFULLBODYIK_DEBUG_CONSTRUCT_TIME
			float configureBoneEndTime = Time.realtimeSinceStartup;
#endif
			Prepare();
#if SAFULLBODYIK_DEBUG_CONSTRUCT_TIME
			float prepareEndTime = Time.realtimeSinceStartup;
			Debug.Log( "Total time: " + (prepareEndTime - constructBeginTime) + " _Prefix():" + (prefixEndTime - constructBeginTime) + " ConfigureBoneTransforms():" + (configureBoneEndTime - prefixEndTime) + " Prepare():" + (prepareEndTime - configureBoneEndTime) );
#endif
		}

		public void Destroy()
		{
			if( _effectors != null ) {
				for( int i = 0; i < _effectors.Length; ++i ) {
					if( _effectors[i].transform != null ) {
						GameObject.DestroyImmediate( _effectors[i].transform.gameObject );
					}
                }
			}
		}

		static void _SetBoneTransform( ref Bone bone, Transform transform )
		{
			if( bone == null ) {
				bone = new Bone();
			}

			bone.transform = transform;
		}

		static void _SetFingerBoneTransform( ref Bone[] bones, Transform[,] transforms, int index )
		{
			if( bones == null || bones.Length != MaxHandFingerLength ) {
				bones = new Bone[MaxHandFingerLength];
			}

			for( int i = 0; i != MaxHandFingerLength; ++i ) {
				if( bones[i] == null ) {
					bones[i] = new Bone();
				}
				bones[i].transform = transforms[index, i];
			}
		}

		static bool _IsNeck( Transform trn )
		{
			if( trn != null ) {
				string name = trn.name;
				if( name != null ) {
					if( name.Contains( "Neck" ) || name.Contains( "neck" ) || name.Contains( "NECK" ) ) {
						return true;
					}
					if( name.Contains( "Kubi" ) || name.Contains( "kubi" ) || name.Contains( "KUBI" ) ) {
						return true;
					}
					if( name.Contains( "\u304F\u3073" ) ) { // Kubi(Hira-gana)
						return true;
					}
					if( name.Contains( "\u30AF\u30D3" ) ) { // Kubi(Kana-kana)
						return true;
					}
					if( name.Contains( "\u9996" ) ) { // Kubi(Kanji)
						return true;
					}
				}
			}

			return false;
		}

		// - Call from Editor script.
		public void Prefix( Transform rootTransform_ )
		{
			if( rootTransform != rootTransform_ ) {
				rootTransform = rootTransform_;
            }

			_Prefix();
		}

		// - Call from FullBodyIKBehaviour.Awake() / FullBodyIK.Initialize().
		// - Bone transforms are null yet.
		void _Prefix()
		{
			if( _isPrefixed ) {
				return;
			}

			_isPrefixed = true;

			SafeNew( ref bodyBones );
			SafeNew( ref headBones );
			SafeNew( ref leftLegBones );
			SafeNew( ref rightLegBones );

			SafeNew( ref leftArmBones );
			leftArmBones.Repair();
			SafeNew( ref rightArmBones );
			rightArmBones.Repair();

			SafeNew( ref leftHandFingersBones );
			leftHandFingersBones.Repair();
			SafeNew( ref rightHandFingersBones );
			rightHandFingersBones.Repair();

			SafeNew( ref bodyEffectors );
			SafeNew( ref headEffectors );
			SafeNew( ref leftArmEffectors );
			SafeNew( ref rightArmEffectors );
			SafeNew( ref leftLegEffectors );
			SafeNew( ref rightLegEffectors );

			SafeNew( ref settings );
			SafeNew( ref editorSettings );
			SafeNew( ref internalValues );

			settings.Prefix();

			if( _bones == null || _bones.Length != (int)BoneLocation.Max ) {
				_bones = new Bone[(int)BoneLocation.Max];
			}
			if( _effectors == null || _effectors.Length != (int)EffectorLocation.Max ) {
				_effectors = new Effector[(int)EffectorLocation.Max];
			}

			_Prefix( ref bodyBones.hips, BoneLocation.Hips, null );
			_Prefix( ref bodyBones.spine, BoneLocation.Spine, bodyBones.hips );
			_Prefix( ref bodyBones.spine2, BoneLocation.Spine2, bodyBones.spine );
			_Prefix( ref headBones.neck, BoneLocation.Neck, bodyBones.spine2 );
			_Prefix( ref headBones.head, BoneLocation.Head, headBones.neck );
			_Prefix( ref headBones.leftEye, BoneLocation.LeftEye, headBones.head );
			_Prefix( ref headBones.rightEye, BoneLocation.RightEye, headBones.head );
			for( int i = 0; i < 2; ++i ) {
				var legBones = (i == 0) ? leftLegBones : rightLegBones;
				_Prefix( ref legBones.leg, (i == 0) ? BoneLocation.LeftLeg : BoneLocation.RightLeg, bodyBones.hips );
				_Prefix( ref legBones.knee, (i == 0) ? BoneLocation.LeftKnee : BoneLocation.RightKnee, legBones.leg );
				_Prefix( ref legBones.foot, (i == 0) ? BoneLocation.LeftFoot : BoneLocation.RightFoot, legBones.knee );

				var armBones = (i == 0) ? leftArmBones : rightArmBones;
				_Prefix( ref armBones.shoulder, (i == 0) ? BoneLocation.LeftShoulder : BoneLocation.RightShoulder, bodyBones.spine2 );
				_Prefix( ref armBones.arm, (i == 0) ? BoneLocation.LeftArm : BoneLocation.RightArm, armBones.shoulder );
				_Prefix( ref armBones.elbow, (i == 0) ? BoneLocation.LeftElbow : BoneLocation.RightElbow, armBones.arm );
				_Prefix( ref armBones.wrist, (i == 0) ? BoneLocation.LeftWrist : BoneLocation.RightWrist, armBones.elbow );

				for( int n = 0; n < MaxArmRollLength; ++n ) {
					var armRollLocation = (i == 0) ? BoneLocation.LeftArmRoll0 : BoneLocation.RightArmRoll0;
					_Prefix( ref armBones.armRoll[n], (BoneLocation)((int)armRollLocation + n), armBones.arm );
				}

				for( int n = 0; n < MaxElbowRollLength; ++n ) {
					var elbowRollLocation = (i == 0) ? BoneLocation.LeftElbowRoll0 : BoneLocation.RightElbowRoll0;
					_Prefix( ref armBones.elbowRoll[n], (BoneLocation)((int)elbowRollLocation + n), armBones.elbow );
				}

				var fingerBones = (i == 0) ? leftHandFingersBones : rightHandFingersBones;
				for( int n = 0; n < MaxHandFingerLength; ++n ) {
					var thumbLocation = (i == 0) ? BoneLocation.LeftHandThumb0 : BoneLocation.RightHandThumb0;
					var indexLocation = (i == 0) ? BoneLocation.LeftHandIndex0 : BoneLocation.RightHandIndex0;
					var middleLocation = (i == 0) ? BoneLocation.LeftHandMiddle0 : BoneLocation.RightHandMiddle0;
					var ringLocation = (i == 0) ? BoneLocation.LeftHandRing0 : BoneLocation.RightHandRing0;
					var littleLocation = (i == 0) ? BoneLocation.LeftHandLittle0 : BoneLocation.RightHandLittle0;
					_Prefix( ref fingerBones.thumb[n], (BoneLocation)((int)thumbLocation + n), (n == 0) ? armBones.wrist : fingerBones.thumb[n - 1] );
					_Prefix( ref fingerBones.index[n], (BoneLocation)((int)indexLocation + n), (n == 0) ? armBones.wrist : fingerBones.index[n - 1] );
					_Prefix( ref fingerBones.middle[n], (BoneLocation)((int)middleLocation + n), (n == 0) ? armBones.wrist : fingerBones.middle[n - 1] );
					_Prefix( ref fingerBones.ring[n], (BoneLocation)((int)ringLocation + n), (n == 0) ? armBones.wrist : fingerBones.ring[n - 1] );
					_Prefix( ref fingerBones.little[n], (BoneLocation)((int)littleLocation + n), (n == 0) ? armBones.wrist : fingerBones.little[n - 1] );
				}
			}

			_Prefix( ref rootEffector, EffectorLocation.Root );
			_Prefix( ref bodyEffectors.hips, EffectorLocation.Hips, rootEffector, bodyBones.hips, leftLegBones.leg, rightLegBones.leg );
			_Prefix( ref headEffectors.neck, EffectorLocation.Neck, bodyEffectors.hips, headBones.neck );
			_Prefix( ref headEffectors.head, EffectorLocation.Head, headEffectors.neck, headBones.head );
			_Prefix( ref headEffectors.eyes, EffectorLocation.Eyes, rootEffector, headBones.head, headBones.leftEye, headBones.rightEye );

			_Prefix( ref leftLegEffectors.knee, EffectorLocation.LeftKnee, rootEffector, leftLegBones.knee );
			_Prefix( ref leftLegEffectors.foot, EffectorLocation.LeftFoot, rootEffector, leftLegBones.foot );
			_Prefix( ref rightLegEffectors.knee, EffectorLocation.RightKnee, rootEffector, rightLegBones.knee );
			_Prefix( ref rightLegEffectors.foot, EffectorLocation.RightFoot, rootEffector, rightLegBones.foot );

			_Prefix( ref leftArmEffectors.arm, EffectorLocation.LeftArm, bodyEffectors.hips, leftArmBones.arm );
			_Prefix( ref leftArmEffectors.elbow, EffectorLocation.LeftElbow, bodyEffectors.hips, leftArmBones.elbow );
			_Prefix( ref leftArmEffectors.wrist, EffectorLocation.LeftWrist, bodyEffectors.hips, leftArmBones.wrist );
			_Prefix( ref rightArmEffectors.arm, EffectorLocation.RightArm, bodyEffectors.hips, rightArmBones.arm );
			_Prefix( ref rightArmEffectors.elbow, EffectorLocation.RightElbow, bodyEffectors.hips, rightArmBones.elbow );
			_Prefix( ref rightArmEffectors.wrist, EffectorLocation.RightWrist, bodyEffectors.hips, rightArmBones.wrist );

			_Prefix( ref leftHandFingersEffectors.thumb, EffectorLocation.LeftHandThumb, leftArmEffectors.wrist, leftHandFingersBones.thumb );
			_Prefix( ref leftHandFingersEffectors.index, EffectorLocation.LeftHandIndex, leftArmEffectors.wrist, leftHandFingersBones.index );
			_Prefix( ref leftHandFingersEffectors.middle, EffectorLocation.LeftHandMiddle, leftArmEffectors.wrist, leftHandFingersBones.middle );
			_Prefix( ref leftHandFingersEffectors.ring, EffectorLocation.LeftHandRing, leftArmEffectors.wrist, leftHandFingersBones.ring );
			_Prefix( ref leftHandFingersEffectors.little, EffectorLocation.LeftHandLittle, leftArmEffectors.wrist, leftHandFingersBones.little );

			_Prefix( ref rightHandFingersEffectors.thumb, EffectorLocation.RightHandThumb, rightArmEffectors.wrist, rightHandFingersBones.thumb );
			_Prefix( ref rightHandFingersEffectors.index, EffectorLocation.RightHandIndex, rightArmEffectors.wrist, rightHandFingersBones.index );
			_Prefix( ref rightHandFingersEffectors.middle, EffectorLocation.RightHandMiddle, rightArmEffectors.wrist, rightHandFingersBones.middle );
			_Prefix( ref rightHandFingersEffectors.ring, EffectorLocation.RightHandRing, rightArmEffectors.wrist, rightHandFingersBones.ring );
			_Prefix( ref rightHandFingersEffectors.little, EffectorLocation.RightHandLittle, rightArmEffectors.wrist, rightHandFingersBones.little );

			// Hidden function.
			Assert( rootTransform != null );
			if( rootTransform != null ) {
				if( settings.modelTemplate == ModelTemplate.Standard ) {
					var animator = rootTransform.GetComponent<Animator>();
					if( animator != null ) {
						var avatar = animator.avatar;
						if( avatar != null ) {
							if( avatar.name.Contains( "unitychan" ) ) {
								settings.modelTemplate = ModelTemplate.UnityChan;
							}
						}
					}
				}
			}

			if( !_isPrefixedAtLeastOnce ) {
				_isPrefixedAtLeastOnce = true;
				_PresetEffectorPull( leftArmEffectors.wrist );
				_PresetEffectorPull( rightArmEffectors.wrist );
				_PresetEffectorPull( leftLegEffectors.foot );
				_PresetEffectorPull( rightLegEffectors.foot );
			}
		}

		static void _PresetEffectorPull( Effector effector )
		{
			if( effector != null ) {
				effector.positionEnabled = true;
				effector.pull = 1.0f;
            }
		}

		public void CleanupBoneTransforms()
		{
			_Prefix();

			if( _bones != null ) {
				for( int i = 0; i < _bones.Length; ++i ) {
					Assert( _bones[i] != null );
					if( _bones[i] != null ) {
						_bones[i].transform = null;
					}
				}
			}
		}

		static readonly string[] _LeftKeywords = new string[]
		{
			"left",
			"_l",
		};

		static readonly string[] _RightKeywords = new string[]
		{
			"right",
			"_r",
		};

		static Transform _FindEye( Transform head, bool isRight )
		{
			if( head != null ) {
				string[] keywords = isRight ? _RightKeywords : _LeftKeywords;

				int childCount = head.childCount;
				for( int i = 0; i < childCount; ++i ) {
					Transform child = head.GetChild( i );
					if( child != null ) {
						string name = child.name;
						if( name != null ) {
							name = name.ToLower();
							if( name != null && name.Contains( "eye" ) ) {
								for( int n = 0; n < keywords.Length; ++n ) {
									if( name.Contains( keywords[n] ) ) {
										return child;
									}
								}
							}
						}
					}
				}
			}

			return null;
		}

		public void ConfigureBoneTransforms()
		{
			_Prefix();

			Assert( settings != null && rootTransform != null );
			if( settings.automaticPrepareHumanoid && rootTransform != null ) {
				Animator animator = rootTransform.GetComponent<Animator>();
				if( animator != null && animator.isHuman ) {
					Transform hips = animator.GetBoneTransform( HumanBodyBones.Hips );
					Transform spine = animator.GetBoneTransform( HumanBodyBones.Spine );
					Transform spine2 = animator.GetBoneTransform( HumanBodyBones.Chest );
					Transform neck = animator.GetBoneTransform( HumanBodyBones.Neck );
					Transform head = animator.GetBoneTransform( HumanBodyBones.Head );
					Transform leftEye = animator.GetBoneTransform( HumanBodyBones.LeftEye );
					Transform rightEye = animator.GetBoneTransform( HumanBodyBones.RightEye );
					Transform leftLeg = animator.GetBoneTransform( HumanBodyBones.LeftUpperLeg );
					Transform rightLeg = animator.GetBoneTransform( HumanBodyBones.RightUpperLeg );
					Transform leftKnee = animator.GetBoneTransform( HumanBodyBones.LeftLowerLeg );
					Transform rightKnee = animator.GetBoneTransform( HumanBodyBones.RightLowerLeg );
					Transform leftFoot = animator.GetBoneTransform( HumanBodyBones.LeftFoot );
					Transform rightFoot = animator.GetBoneTransform( HumanBodyBones.RightFoot );
					Transform leftShoulder = animator.GetBoneTransform( HumanBodyBones.LeftShoulder );
					Transform rightShoulder = animator.GetBoneTransform( HumanBodyBones.RightShoulder );
					Transform leftArm = animator.GetBoneTransform( HumanBodyBones.LeftUpperArm );
					Transform rightArm = animator.GetBoneTransform( HumanBodyBones.RightUpperArm );
					Transform leftElbow = animator.GetBoneTransform( HumanBodyBones.LeftLowerArm );
					Transform rightElbow = animator.GetBoneTransform( HumanBodyBones.RightLowerArm );
					Transform leftWrist = animator.GetBoneTransform( HumanBodyBones.LeftHand );
					Transform rightWrist = animator.GetBoneTransform( HumanBodyBones.RightHand );
					Transform[,] leftFingers = new Transform[5, 4];
					Transform[,] rightFingers = new Transform[5, 4];
					for( int n = 0; n < 2; ++n ) {
						int humanBodyBones = ((n == 0) ? (int)HumanBodyBones.LeftThumbProximal : (int)HumanBodyBones.RightThumbProximal);
						Transform[,] fingers = ((n == 0) ? leftFingers : rightFingers);
						for( int i = 0; i < 5; ++i ) {
							for( int j = 0; j < 3; ++j, ++humanBodyBones ) {
								fingers[i, j] = animator.GetBoneTransform( (HumanBodyBones)humanBodyBones );
							}
							// Fix for tips.
							if( fingers[i, 2] != null && fingers[i, 2].childCount != 0 ) {
								fingers[i, 3] = fingers[i, 2].GetChild( 0 );
							}
						}
					}

					if( neck == null ) {
						if( head != null ) {
							Transform t = head.parent;
							if( t != null && _IsNeck( t ) ) {
								neck = t;
							} else {
								neck = head; // Failsafe.
							}
						}
					}

					if( leftEye == null ) {
						leftEye = _FindEye( head, false );
					}
					if( rightEye == null ) {
						rightEye = _FindEye( head, true );
					}

					_SetBoneTransform( ref bodyBones.hips, hips );
					_SetBoneTransform( ref bodyBones.spine, spine );
					_SetBoneTransform( ref bodyBones.spine2, spine2 );

					_SetBoneTransform( ref headBones.neck, neck );
					_SetBoneTransform( ref headBones.head, head );
					_SetBoneTransform( ref headBones.leftEye, leftEye );
					_SetBoneTransform( ref headBones.rightEye, rightEye );

					_SetBoneTransform( ref leftLegBones.leg, leftLeg );
					_SetBoneTransform( ref leftLegBones.knee, leftKnee );
					_SetBoneTransform( ref leftLegBones.foot, leftFoot );
					_SetBoneTransform( ref rightLegBones.leg, rightLeg );
					_SetBoneTransform( ref rightLegBones.knee, rightKnee );
					_SetBoneTransform( ref rightLegBones.foot, rightFoot );

					_SetBoneTransform( ref leftArmBones.shoulder, leftShoulder );
					_SetBoneTransform( ref leftArmBones.arm, leftArm );
					_SetBoneTransform( ref leftArmBones.elbow, leftElbow );
					_SetBoneTransform( ref leftArmBones.wrist, leftWrist );
					_SetBoneTransform( ref rightArmBones.shoulder, rightShoulder );
					_SetBoneTransform( ref rightArmBones.arm, rightArm );
					_SetBoneTransform( ref rightArmBones.elbow, rightElbow );
					_SetBoneTransform( ref rightArmBones.wrist, rightWrist );

					_SetFingerBoneTransform( ref leftHandFingersBones.thumb, leftFingers, 0 );
					_SetFingerBoneTransform( ref leftHandFingersBones.index, leftFingers, 1 );
					_SetFingerBoneTransform( ref leftHandFingersBones.middle, leftFingers, 2 );
					_SetFingerBoneTransform( ref leftHandFingersBones.ring, leftFingers, 3 );
					_SetFingerBoneTransform( ref leftHandFingersBones.little, leftFingers, 4 );

					_SetFingerBoneTransform( ref rightHandFingersBones.thumb, rightFingers, 0 );
					_SetFingerBoneTransform( ref rightHandFingersBones.index, rightFingers, 1 );
					_SetFingerBoneTransform( ref rightHandFingersBones.middle, rightFingers, 2 );
					_SetFingerBoneTransform( ref rightHandFingersBones.ring, rightFingers, 3 );
					_SetFingerBoneTransform( ref rightHandFingersBones.little, rightFingers, 4 );
				}
			}

			if( settings.automaticConfigureRollEnabled ) {
				var tempBones = new List<Transform>();

				for( int side = 0; side < 2; ++side ) {
					var armBones = (side == 0) ? leftArmBones : rightArmBones;
					if( armBones != null &&
						armBones.arm != null && armBones.arm.transform != null &&
						armBones.elbow != null && armBones.elbow.transform != null &&
						armBones.wrist != null && armBones.wrist.transform != null ) {

						_ConfigureRollBones( armBones.armRoll, tempBones, armBones.arm.transform, armBones.elbow.transform, (Side)side, true );
						_ConfigureRollBones( armBones.elbowRoll, tempBones, armBones.elbow.transform, armBones.wrist.transform, (Side)side, false );
					}
				}
			}
		}

		void _ConfigureRollBones( Bone[] bones, List<Transform> tempBones, Transform transform, Transform excludeTransform, Side side, bool isArm )
		{
			bool isRollSpecial = false;
			string rollSpecialName = null;
			if( isArm ) {
				rollSpecialName = (side == Side.Left) ? "LeftArmRoll" : "RightArmRoll";
			} else {
				rollSpecialName = (side == Side.Left) ? "LeftElbowRoll" : "RightElbowRoll";
			}

			int childCount = transform.childCount;

			for( int i = 0; i < childCount; ++i ) {
				var childTransform = transform.GetChild( i );
				var name = childTransform.name;
				if( name != null && name.Contains( rollSpecialName ) ) {
					isRollSpecial = true;
					break;
				}
			}

			tempBones.Clear();

			for( int i = 0; i < childCount; ++i ) {
				var childTransform = transform.GetChild( i );
				var name = childTransform.name;
				if( name != null ) {
					if( excludeTransform != childTransform &&
						!excludeTransform.IsChildOf( childTransform ) ) {
						if( isRollSpecial ) {
							if( name.Contains( rollSpecialName ) ) {
								char nameEnd = name[name.Length - 1];
								if( nameEnd >= '0' && nameEnd <= '9' ) {
									tempBones.Add( childTransform );
								}
							}
						} else {
							tempBones.Add( childTransform );
						}
					}
				}
			}

			childCount = Mathf.Min( tempBones.Count, bones.Length );
			for( int i = 0; i < childCount; ++i ) {
				_SetBoneTransform( ref bones[i], tempBones[i] );
			}
		}

		// - Wakeup for solvers.
		// - Require to setup each transforms.
		public void Prepare()
		{
			_Prefix();

			Assert( rootTransform != null );
			if( rootTransform != null ) { // Failsafe.
				internalValues.defaultRootPosition = rootTransform.position;
				internalValues.defaultRootBasis = Matrix3x3.FromColumn( rootTransform.right, rootTransform.up, rootTransform.forward );
				internalValues.defaultRootBasisInv = internalValues.defaultRootBasis.transpose;
				internalValues.defaultRootRotation = rootTransform.rotation;
			}

			if( _bones != null ) {
				for( int i = 0; i < _bones.Length; ++i ) {
					Assert( _bones[i] != null );
					if( _bones[i] != null ) {
						_bones[i].Prepare( this );
					}
				}
				for( int i = 0; i < _bones.Length; ++i ) {
					Assert( _bones[i] != null );
					if( _bones[i] != null ) {
						_bones[i].PostPrepare();
					}
				}
			}

			if( _effectors != null ) {
				for( int i = 0; i < _effectors.Length; ++i ) {
					Assert( _effectors[i] != null );
					if( _effectors[i] != null ) {
						_effectors[i].Prepare( this );
					}
				}
			}

			_bodyIK = new BodyIK( this );

			if( _limbIK == null || _limbIK.Length != (int)LimbIKLocation.Max ) {
				_limbIK = new LimbIK[(int)LimbIKLocation.Max];
			}

			for( int i = 0; i < _limbIK.Length; ++i ) {
				_limbIK[i] = new LimbIK( this, (LimbIKLocation)i );
			}

			_headIK = new HeadIK( this );

			if( _fingerIK == null || _fingerIK.Length != (int)FingerIKType.Max ) {
				_fingerIK = new FingerIK[(int)FingerIKType.Max];
			}

			for( int i = 0; i < _fingerIK.Length; ++i ) {
				_fingerIK[i] = new FingerIK( this, (FingerIKType)i );
			}
		}

		bool _isAnimatorCheckedAtLeastOnce = false;

		void _UpdateInternalValues()
		{
			// _animatorEnabledImmediately
			if( settings.animatorEnabled == AutomaticBool.Auto ) {
				if( !_isAnimatorCheckedAtLeastOnce ) {
					_isAnimatorCheckedAtLeastOnce = true;
					internalValues.animatorEnabled = false;
					if( rootTransform != null ) {
						var animator = rootTransform.GetComponent<Animator>();
						if( animator != null && animator.enabled ) {
							var runtimeAnimatorController = animator.runtimeAnimatorController;
							internalValues.animatorEnabled = (runtimeAnimatorController != null);
						}
						if( animator == null ) { // Legacy support.
							var animation = rootTransform.GetComponent<Animation>();
							if( animation != null && animation.enabled && animation.GetClipCount() > 0 ) {
								internalValues.animatorEnabled = true;
							}
						}
					}
				}
			} else {
				internalValues.animatorEnabled = (settings.animatorEnabled != AutomaticBool.Disable);
				_isAnimatorCheckedAtLeastOnce = false;
			}

			if( settings.resetTransforms == AutomaticBool.Auto ) {
				internalValues.resetTransforms = !(internalValues.animatorEnabled);
			} else {
				internalValues.resetTransforms = (settings.resetTransforms != AutomaticBool.Disable);
			}

			internalValues.continuousSolverEnabled = !internalValues.animatorEnabled && !internalValues.resetTransforms;

			internalValues.bodyIK.Update( settings.bodyIK );
			internalValues.limbIK.Update( settings.limbIK );
		}

		public void Update()
		{
			_UpdateInternalValues();

			if( _effectors != null ) {
				for( int i = 0; i < _effectors.Length; ++i ) {
					if( _effectors[i] != null ) {
						_effectors[i].PrepareUpdate();
					}
				}
			}

			internalValues.ClearDegugPoints();

			_Bones_PrepareUpdate();

			// Feedback bonePositions to effectorPositions.
			// (for AnimatorEnabled only.)
			if( _effectors != null ) {
				for( int i = 0; i < _effectors.Length; ++i ) {
					if( _effectors[i] != null ) {
						_effectors[i]._hidden_worldPosition = _effectors[i].worldPosition;

						if( internalValues.animatorEnabled && !internalValues.resetTransforms ) {
							if( _effectors[i].positionEnabled && _effectors[i].positionWeight < 1.0f - IKEpsilon ) {
								float weight = (_effectors[i].positionWeight > IKEpsilon) ? _effectors[i].positionWeight : 0.0f;
								if( _effectors[i].bone != null && _effectors[i].bone.transformIsAlive ) {
									_effectors[i]._hidden_worldPosition = Vector3.Lerp( _effectors[i].bone.worldPosition, _effectors[i].worldPosition, weight );
								}
							} else if( !_effectors[i].positionEnabled ) {
								if( _effectors[i].bone != null && _effectors[i].bone.transformIsAlive ) {
									_effectors[i]._hidden_worldPosition = _effectors[i].bone.worldPosition;
								}
							}
						}
					}
				}
			}

			// Presolve locations.
			for( int i = 0; i < _limbIK.Length; ++i ) {
				_limbIK[i].PresolveBeinding();
			}

			if( _bodyIK != null ) {
				if( _bodyIK.Solve() ) {
					_Bones_WriteToTransform();
				}
			}

			if( _limbIK != null || _headIK != null ) {
				_Bones_PrepareUpdate();

				bool isSolved = false;
				if( _limbIK != null ) {
					for( int i = 0; i < _limbIK.Length; ++i ) {
						if( _limbIK[i] != null ) {
							isSolved |= _limbIK[i].Solve();
						}
					}
				}
				if( _headIK != null ) {
					_headIK.Solve();
					isSolved = true;
				}

				if( isSolved ) {
					_Bones_WriteToTransform();
				}
			}

			if( _fingerIK != null ) {
				_Bones_PrepareUpdate();

				bool isSolved = false;
				for( int i = 0; i < _fingerIK.Length; ++i ) {
					isSolved |= _fingerIK[i].Solve();
				}

				if( isSolved ) {
					_Bones_WriteToTransform();
				}
			}
		}

		void _Bones_PrepareUpdate()
		{
			if( _bones != null ) {
				for( int i = 0; i < _bones.Length; ++i ) {
					if( _bones[i] != null ) {
						_bones[i].PrepareUpdate();
					}
				}
			}
		}

		void _Bones_WriteToTransform()
		{
			if( _bones != null ) {
				for( int i = 0; i < _bones.Length; ++i ) {
					if( _bones[i] != null ) {
						_bones[i].WriteToTransform();
					}
				}
			}
		}

#if UNITY_EDITOR
		public void DrawGizmos()
		{
			Vector3 cameraForward = Camera.current.transform.forward;

			if( _effectors != null ) {
				for( int i = 0; i != _effectors.Length; ++i ) {
					_DrawEffectorGizmo( _effectors[i] );
				}
			}

			if( _bones != null ) {
				for( int i = 0; i != _bones.Length; ++i ) {
					_DrawBoneGizmo( _bones[i], ref cameraForward );
				}
			}

#if SAFULLBODYIK_DEBUG
			if( internalValues != null && internalValues.debugData != null ) {
				var debugPoints = internalValues.debugData.debugPoints;
				for( int i = 0; i < debugPoints.Count; ++i ) {
					Gizmos.color = debugPoints[i].color;
					for( int n = 0; n < 8; ++n ) {
						Gizmos.DrawWireSphere( debugPoints[i].pos, debugPoints[i].size );
					}
				}
			}
#endif
		}

		const float _EffectorGizmoSize = 0.04f;
		const float _FingerEffectorGizmoSize = 0.02f;

		static void _DrawEffectorGizmo( Effector effector )
		{
			if( effector != null ) {
				bool isFinger = (effector.bone != null && effector.bone.boneType == BoneType.HandFinger);
				float effectorSize = (isFinger ? _FingerEffectorGizmoSize : _EffectorGizmoSize);
				Gizmos.color = Color.green;

				Vector3 position = Vector3.zero;
				if( effector.transform != null ) {
					position = effector.transform.position;
				} else {
					position = effector._worldPosition; // Memo: Don't re-write internal flags. (Use _worldPosition directly.)
				}

				Gizmos.DrawWireSphere( position, effectorSize );
				Gizmos.DrawWireSphere( position, effectorSize );
				Gizmos.DrawWireSphere( position, effectorSize );
				Gizmos.DrawWireSphere( position, effectorSize );
			}
		}

		void _DrawBoneGizmo( Bone bone, ref Vector3 cameraForward )
		{
			if( bone == null || bone.transform == null ) {
				return;
			}

			if( bone.boneType == BoneType.Eye ) {
				if( settings.modelTemplate == ModelTemplate.UnityChan ) {
					return;
				}
			}

			Transform parentTransform = bone.parentTransform;

			Vector3 position = bone.transform.position;
			Quaternion rotation = bone.transform.rotation * bone._worldToBoneRotation;
			Matrix3x3 basis;
			SAFBIKMatSetRot( out basis, ref rotation );

			_DrawTransformGizmo( position, ref basis, ref cameraForward, bone.boneType );

			if( parentTransform != null ) {
				Gizmos.color = Color.white;

				rotation = parentTransform.rotation * bone.parentBone._worldToBoneRotation;
				SAFBIKMatSetRot( out basis, ref rotation );

				_DrawBoneGizmo( parentTransform.position, position, ref basis, ref cameraForward, bone.boneType );
			}
		}

		static void _DrawTransformGizmo( Vector3 position, ref Matrix3x3 basis, ref Vector3 cameraForward, BoneType boneType )
		{
			Vector3 column0 = basis.column0;
			Vector3 column1 = basis.column1;
			Vector3 column2 = basis.column2;

			// X Axis
			_DrawArrowGizmo( Color.red, ref position, ref column0, ref cameraForward, boneType );
			// Y Axis
			_DrawArrowGizmo( Color.green, ref position, ref column1, ref cameraForward, boneType );
			// Z Axis
			_DrawArrowGizmo( Color.blue, ref position, ref column2, ref cameraForward, boneType );
		}

		const float _ArrowGizmoLowerLength = 0.02f;
		const float _ArrowGizmoLowerWidth = 0.003f;
		const float _ArrowGizmoMiddleLength = 0.0025f;
		const float _ArrowGizmoMiddleWidth = 0.0075f;
		const float _ArrowGizmoUpperLength = 0.05f;
		const float _ArrowGizmoThickness = 0.0002f;
		const int _ArrowGizmoDrawCycles = 8;
		const float _ArrowGizmoEyeScale = 0.5f;
		const float _ArrowGizmoFingerScale = 0.1f;

		static void _DrawArrowGizmo(
			Color color,
			ref Vector3 position,
			ref Vector3 direction,
			ref Vector3 cameraForward,
			BoneType boneType )
		{
			Vector3 nY = Vector3.Cross( cameraForward, direction );
			if( SAFBIKVecNormalize( ref nY ) ) {
				Gizmos.color = color;

				float arrowGizmoLowerLength = _ArrowGizmoLowerLength;
				float arrowGizmoLowerWidth = _ArrowGizmoLowerWidth;
				float arrowGizmoMiddleLength = _ArrowGizmoMiddleLength;
				float arrowGizmoMiddleWidth = _ArrowGizmoMiddleWidth;
				float arrowGizmoUpperLength = _ArrowGizmoUpperLength;
				float arrowGizmoThickness = _ArrowGizmoThickness;

				float gizmoScale = 1.0f;
				if( boneType == BoneType.HandFinger ) {
					gizmoScale = _ArrowGizmoFingerScale;
				}
				if( boneType == BoneType.Eye ) {
					gizmoScale = _ArrowGizmoEyeScale;
				}
				if( gizmoScale != 1.0f ) {
					arrowGizmoLowerLength = _ArrowGizmoLowerLength * gizmoScale;
					arrowGizmoLowerWidth = _ArrowGizmoLowerWidth * gizmoScale;
					arrowGizmoMiddleLength = _ArrowGizmoMiddleLength * gizmoScale;
					arrowGizmoMiddleWidth = _ArrowGizmoMiddleWidth * gizmoScale;
					arrowGizmoUpperLength = _ArrowGizmoUpperLength * gizmoScale;
					arrowGizmoThickness = _ArrowGizmoThickness * gizmoScale;
				}

				Vector3 posLower = position + direction * arrowGizmoLowerLength;
				Vector3 posMiddle = position + direction * (arrowGizmoLowerLength + arrowGizmoMiddleLength);
				Vector3 posUpper = position + direction * (arrowGizmoLowerLength + arrowGizmoMiddleLength + arrowGizmoUpperLength);

				Vector3 lowerY = nY * arrowGizmoLowerWidth;
				Vector3 middleY = nY * arrowGizmoMiddleWidth;
				Vector3 thicknessY = nY * arrowGizmoThickness;

				for( int i = 0; i < _ArrowGizmoDrawCycles; ++i ) {
					Gizmos.DrawLine( position, posLower + lowerY );
					Gizmos.DrawLine( position, posLower - lowerY );
					Gizmos.DrawLine( posLower + lowerY, posMiddle + middleY );
					Gizmos.DrawLine( posLower - lowerY, posMiddle - middleY );
					Gizmos.DrawLine( posMiddle + middleY, posUpper );
					Gizmos.DrawLine( posMiddle - middleY, posUpper );
					lowerY -= thicknessY;
					middleY -= thicknessY;
				}
			}
		}

		const float _BoneGizmoOuterLen = 0.015f;
		const float _BoneGizmoThickness = 0.0003f;
		const int _BoneGizmoDrawCycles = 8;
		const float _BoneGizmoFingerScale = 0.25f;

		static void _DrawBoneGizmo( Vector3 fromPosition, Vector3 toPosition, ref Matrix3x3 basis, ref Vector3 cameraForward, BoneType boneType )
		{
			Vector3 dir = toPosition - fromPosition;
			if( SAFBIKVecNormalize( ref dir ) ) {
				Vector3 nY = Vector3.Cross( cameraForward, dir );
				if( SAFBIKVecNormalize( ref nY ) ) {
					float boneGizmoOuterLen = _BoneGizmoOuterLen;
					float boneGizmoThickness = _BoneGizmoThickness;
					if( boneType == BoneType.HandFinger ) {
						boneGizmoOuterLen = _BoneGizmoOuterLen * _BoneGizmoFingerScale;
						boneGizmoThickness = _BoneGizmoThickness * _BoneGizmoFingerScale;
					}

					Vector3 outerY = nY * boneGizmoOuterLen;
					Vector3 thicknessY = nY * boneGizmoThickness;
					Vector3 interPosition = fromPosition + dir * boneGizmoOuterLen;

					for( int i = 0; i < _BoneGizmoDrawCycles; ++i ) {
						Gizmos.color = (i < _BoneGizmoDrawCycles / 2) ? Color.black : Color.white;
						Gizmos.DrawLine( fromPosition, interPosition + outerY );
						Gizmos.DrawLine( fromPosition, interPosition - outerY );
						Gizmos.DrawLine( interPosition + outerY, toPosition );
						Gizmos.DrawLine( interPosition - outerY, toPosition );
						outerY -= thicknessY;
					}
				}
			}
		}
#endif

		void _Prefix( ref Bone bone, BoneLocation boneLocation, Bone parentBoneLocationBased )
		{
			Assert( _bones != null );
			Bone.Prefix( _bones, ref bone, boneLocation, parentBoneLocationBased );
		}

		void _Prefix(
			ref Effector effector,
			EffectorLocation effectorLocation )
		{
			Assert( _effectors != null );
			bool createEffectorTransform = this.settings.createEffectorTransform;
			Assert( rootTransform != null );
			Effector.Prefix( _effectors, ref effector, effectorLocation, createEffectorTransform, rootTransform );
		}

		void _Prefix(
			ref Effector effector,
			EffectorLocation effectorLocation,
			Effector parentEffector,
			Bone[] bones )
		{
			_Prefix( ref effector, effectorLocation, parentEffector, (bones != null && bones.Length > 0) ? bones[bones.Length - 1] : null );
		}

		void _Prefix(
			ref Effector effector,
			EffectorLocation effectorLocation,
			Effector parentEffector,
			Bone bone,
			Bone leftBone = null,
			Bone rightBone = null )
		{
			Assert( _effectors != null );
			bool createEffectorTransform = this.settings.createEffectorTransform;
			Effector.Prefix( _effectors, ref effector, effectorLocation, createEffectorTransform, null, parentEffector, bone, leftBone, rightBone );
		}
	}
}
