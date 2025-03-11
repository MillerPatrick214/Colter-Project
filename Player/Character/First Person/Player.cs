using Godot;

public partial class Player : CharacterBody3D
{
	public static Player Instance { get; private set; }

	[ExportGroup("Movement Settings")]
	[Export(PropertyHint.None, "suffix:m/s")] public float SprintSpeed = 30; 
	[Export(PropertyHint.None, "suffix:m/s")] public float Speed = 20;
	[Export(PropertyHint.None, "suffix:m/s")] public float JumpManueverSpeed = 15;
	[Export] public float JumpImpulse = 20;

	[ExportSubgroup("Camera Movement Settings")]
	[Export(PropertyHint.None, "suffix:\u00ba")] public float LeanAngle = 30;
	[Export(PropertyHint.None, "suffix:m/s")] public float LeanSpeed = 10;
	[Export(PropertyHint.None, "suffix:%")] float CameraPivotRotation = 10;

	[ExportSubgroup("Item Movement Settings")]
	[Export(PropertyHint.None, "suffix:%")] float ItemSwayAmount = 30;
	[Export(PropertyHint.None, "suffix:%")] float ItemPivotRotation = 20; 
	[Export(PropertyHint.None, "suffix:%")] float BobAmount = 30;
	[Export(PropertyHint.None, "suffix:%")] float BobFrequency = 40;

	[ExportGroup("Camera Settings")]
	[Export(PropertyHint.None, "suffix:m")] float StandingHeight = 1.7f; 
	[Export(PropertyHint.None, "suffix:m")] float CrouchingHeight = 1.0f;
	[Export(PropertyHint.None, "suffix:%")] float StandingCameraPivot = .55f;
	[Export(PropertyHint.None, "suffix:%")] float CrouchingCameraPivot = 0;
	[Export(PropertyHint.None, "suffix:\u00ba")] float YRotationMinimum = -70;
	[Export(PropertyHint.None, "suffix:\u00ba")] float YRotationMaximum = 70;
	
	float lookAroundSpeed;
	CamPivot CamPivNode;
	float mouseRotX = 0;
	float mouseRotY = 0;

	string PlayerDataPath = "uid://ccgu64xutyl85";
	PlayerData PlayerData;
	Marker3D ItemMarker;
	CollisionShape3D CollisionShapeNode;
	Vector2 MouseMotion;
	bool isCrouching;
	bool isInteracting;
	int SlotIndex = 0;
	Item3D Held;
	UI UINode;
	GodotObject ObjectSeen; 
	CapsuleShape3D CapsuleShape;	// We need to access the Shape property of our collisionshape3d and store it here

	public enum LeanDirection {
		Left = 1,
		None = 0,
		Right = -1
	}

	LeanDirection Leaning;
	CanvasLayer UnderWaterCanvasLayer;

	public Inventory Inventory { get; private set; }
 
    public override void _Ready()
    {
		PlayerData = LoadData();
		Instance = this;
		Inventory = PlayerData.PlayerInventory;
		Mathf.Wrap(SlotIndex, 0, 6);
		Leaning = LeanDirection.None;
		ObjectSeen = null;
		Events.Instance.ChangeIsInteracting += (isActive) => InteractMouseMode(isActive);
		InteractMouseMode(false);
		
		ItemMarker = GetNodeOrNull<Marker3D>("CamPivot/PlayerItemMarker");
		CamPivNode = GetNodeOrNull<CamPivot>("CamPivot");
		UINode = GetNodeOrNull<UI>("UI");

		CollisionShapeNode = GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
		InventoryUI InventoryUI = GetNodeOrNull<InventoryUI>("UI/InventoryUI");

		if (InventoryUI == null)
		{
			GD.PrintErr("Error in Character.cs: InventoryUI returned null. Unable to connect Inventory information");
		}

		if (Inventory == null) {
			GD.PrintErr("Error in Character.cs: Inventory returned null");
		}

		if (CollisionShapeNode == null) {GD.PrintErr("Error Player.cs: CollisionShapeNode returned null");}
		
		Events.Instance.PickUp += (item) => Inventory.PickUpItem(item);

		UnderWaterCanvasLayer = GetNodeOrNull<CanvasLayer>("SubViewportContainer/SubViewport/CanvasLayer");
		if (UnderWaterCanvasLayer == null) GD.PrintErr("Player UnderWaterCanvaslayer null. I'm going to freak the fuck out and take a man's life over this shit rn");

		Events.Instance.UnderwaterToggle += (tf) => ToggleUnderWater(tf);

		CapsuleShape = CollisionShapeNode.Shape as CapsuleShape3D;

		lookAroundSpeed = 10.0f; //(float)Settings.Instance.GetSetting("gameplay", "look_sensitivity");
    }

	public void SaveData()
	{
		ResourceSaver.Save(PlayerData, PlayerDataPath);
	}

	public PlayerData LoadData()
	{
		PlayerData = GD.Load<PlayerData>(PlayerDataPath);
		return PlayerData;
	}

	public override void _Process(double delta) {
		int curr_slot = SlotIndex;
		isCrouching = Input.IsActionPressed("Crouch") ? true : false;
		//IsLeaning = LeanDirection.None
		
		if (Input.IsActionPressed("LeanLeft")) { Leaning = LeanDirection.Left;}			//These determine Lean Direction based on input.
		else if(Input.IsActionPressed("LeanRight")) { Leaning = LeanDirection.Right;}
		//else if (Input.IsActionPressed("LeanRight") && Input.IsActionPressed("LeanRight")) { IsLeaning = LeanDirection.None;}
		else {Leaning = LeanDirection.None;}

		//GD.Print(Leaning);
		Lean(Leaning);
		Crouch(isCrouching);

		Equippable item;

		if (Input.IsActionJustPressed("PrimaryWeapon1"))
		{
			item = Inventory.EquipFromSlot(0);
			SlotIndex = 0;
			SetEquipped(item);
		}
		if (Input.IsActionJustPressed("PrimaryWeapon2"))
		{
			item = Inventory.EquipFromSlot(1);
			SlotIndex = 1;
			SetEquipped(item);
		}
		if (Input.IsActionJustPressed("SecondaryWeapon1"))
		{
			item = Inventory.EquipFromSlot(2);
			SlotIndex = 2;
			SetEquipped(item);
		}
		if (Input.IsActionJustPressed("SecondaryWeapon2"))
		{
			item = Inventory.EquipFromSlot(3);
			SlotIndex = 3;
			SetEquipped(item);
		}
		if (Input.IsActionJustPressed("SecondaryWeapon3"))
		{
			item = Inventory.EquipFromSlot(4);
			SlotIndex = 4;
			SetEquipped(item);
		}
		if (Input.IsActionJustPressed("MeleeWeapon"))
		{
			item = Inventory.EquipFromSlot(5);
			SlotIndex = 5;
			SetEquipped(item);
		}
		if (Input.IsActionJustPressed("Utility"))
		{
			item = Inventory.EquipFromSlot(6);
			SlotIndex = 6;
			SetEquipped(item);
		}
		if(Input.IsActionJustPressed("CycleUp"))
		{
			SlotIndex += 1;
			item = Inventory.EquipFromSlot(SlotIndex);
			SetEquipped(item);
		}
		if(Input.IsActionJustPressed("CycleUp"))
		{
			SlotIndex -= 1;
			item = Inventory.EquipFromSlot(SlotIndex);
			SetEquipped(item);
		}

		if (Held != null)
		{
			if (Held is RangedWeapon rangedwep)
			{
				if (Input.IsActionPressed("Aim"))
				{
					rangedwep.SetIsAiming(true);

					if (Input.IsActionJustPressed("UseItem"))
					{
						rangedwep.Fire();
					}
				}

				else
				{
					rangedwep.SetIsAiming(false);
				}
			}
		}

		if (Held is MeleeWeapon meleewep)
		{
			if(Input.IsActionPressed("UseItem"))
			{
				meleewep.Attack();
			}
		}

		Godot.Vector2 input_dir = Input.GetVector("Left", "Right", "Forward", "Back");
        CamTilt(input_dir.X, delta);
		ItemTilt(input_dir.X, delta);
		ItemBob(delta);
		ItemSway(delta);
	}

	public override void _Input(InputEvent @event) {
		if (!isInteracting) {
			if (@event is InputEventMouseMotion mouseMotion) { //mouseMotion is a local variable here
				MouseMotion = mouseMotion.Relative; //Saving this for weaponsway
				mouseRotX += mouseMotion.Relative.X * (lookAroundSpeed / 100);		//Note -- The XY may seen flipped, but it's not. Rotation on the X axis is up and down according to the player.
				mouseRotY -= mouseMotion.Relative.Y * (lookAroundSpeed / 100);
				
				mouseRotY = Mathf.Clamp(mouseRotY, YRotationMinimum, YRotationMaximum);

				Vector3 char_rot = new Godot.Vector3(RotationDegrees.X, -mouseRotX, RotationDegrees.Z); 	
				Vector3 cam_piv_rot = new Godot.Vector3(mouseRotY, CamPivNode.RotationDegrees.Y, CamPivNode.RotationDegrees.Z);
				//Basis item_basis = ItemMarker.GlobalTransform.Basis;
				RotationDegrees = char_rot;
				CamPivNode.RotationDegrees = cam_piv_rot;
			}
		}
	}

    public void SetStartPosition(Godot.Vector3 pos)
	{
		Position = pos;
	}

	public void SetEquipped(Equippable item) 
	{
		if (Held != null)
		{
			if (item == null)
			{
				Held.QueueFree();
				Held = null;
				return;
			}
			Held.QueueFree();
			Held = null;
		}
		else if (item != null)
		{
			var instance = ResourceLoader.Load<PackedScene>(item.ScenePath).Instantiate();
			ItemMarker.AddChild(instance);
			Held = GetNodeOrNull<Item3D>(instance.GetPath());
			Held.SetHeld(true, true);
		}
	}

	public void Crouch(bool isCrouching) {
		float currentHeight = CapsuleShape.Height;
		float currentCamPos = CamPivNode.Position.Y;

		float targetHeight = isCrouching ? CrouchingHeight : StandingHeight;
		//float targetCamPos = isCrouching ? CrouchingCameraPivot : StandingCameraPivot; //TF happened here?
		
		if  (currentHeight != StandingHeight || currentHeight != CrouchingHeight) {
			CapsuleShape.Height = Mathf.Lerp(currentHeight, targetHeight, 0.05f);
			CamPivNode.Position = new Godot.Vector3(CamPivNode.Position.X, Mathf.Lerp(currentCamPos, targetHeight, 0.05f), CamPivNode.Position.Z); 

			if (Mathf.Abs(targetHeight - currentHeight) < 0.01f) {
				CapsuleShape.Height = targetHeight;
				CamPivNode.Position = new Godot.Vector3(0, targetHeight, 0);
			}
		}
	}
	
	public void CamTilt(float input_x, double delta)
	{
		Godot.Vector3 cam_piv_rot = CamPivNode.Rotation;
		float tilt_rot = cam_piv_rot.Z;
		tilt_rot = Godot.Mathf.Lerp(tilt_rot, -input_x * CameraPivotRotation / 1000, 10 * (float)delta);
		cam_piv_rot = new Godot.Vector3(cam_piv_rot.X, cam_piv_rot.Y, tilt_rot);
		CamPivNode.Rotation = cam_piv_rot;
	}
	
	public void ItemTilt(float input_x, double delta)
	{	
		Godot.Vector3 item_rot = ItemMarker.Rotation;
		float tilt_rot = item_rot.Z;
		tilt_rot = Godot.Mathf.Lerp(tilt_rot, -input_x * ItemPivotRotation / 100, 10 * (float)delta);
		item_rot = new Godot.Vector3(item_rot.X, item_rot.Y, tilt_rot);
		ItemMarker.Rotation = item_rot;
	}

	public void ItemSway(double delta)
	{
		MouseMotion = MouseMotion.Lerp(Vector2.Zero, 10 * (float)delta);

		float item_rot_X = ItemMarker.Rotation.X;
		float item_rot_Y = ItemMarker.Rotation.Y;

		item_rot_X = Mathf.Lerp(item_rot_X, MouseMotion.Y * ItemSwayAmount / 10_000, 10 * (float)delta);
		item_rot_Y = Mathf.Lerp(item_rot_Y, MouseMotion.X * ItemSwayAmount / 10_000, 10 * (float)delta);

		ItemMarker.Rotation = new Vector3(item_rot_X, item_rot_Y, ItemMarker.Rotation.Z);
	} 

	public void ItemBob(double delta)
	{
		Vector3 item_pos = ItemMarker.Position;

		if (Velocity != Vector3.Zero)
		{
			item_pos.X = Mathf.Lerp(item_pos.X, 0 + Mathf.Sin(Time.GetTicksMsec() * (BobFrequency / 10_000) * .05f) * (BobAmount / 10_000), 10 * (float)delta);
			item_pos.Y = Mathf.Lerp(item_pos.Y, 0 + Mathf.Sin(Time.GetTicksMsec() * (BobFrequency / 10_000) + .2315f) * (BobAmount / 10_000), 10 * (float)delta);
		}
		
		else 
		{
			item_pos = item_pos.Lerp(Vector3.Zero, 10 * (float) delta);
		}

		ItemMarker.Position = item_pos;
	}

	public void InteractMouseMode(bool isInteracting)
	{
		this.isInteracting = isInteracting;
		if (isInteracting)
		{
			Input.MouseMode = Input.MouseModeEnum.Confined;
		}
		
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}
	
	public void Lean(LeanDirection Leaning) { //This feels very calculation heavy for something that is called each delta
		
		// Once I clean up the cam pivot node, weapon, etc then I will implement the camera spinning the other way to eliminate disoriantation
		// If cam node and player node share a piv point it just lessens the overall lean subtractivly & disconnects player model & camera

		float currentRotation = RotationDegrees.Z;
		float targetRotation = (float)Leaning * LeanAngle;

		float currCamPivRot = CamPivNode.RotationDegrees.Z;
		float targetCamPivRot = -(float)Leaning * (LeanAngle * .5f);		//CamPivot Rotates the opposite direction, x% of the strength of the lean

		float newRotation = Mathf.Wrap(Mathf.Lerp(currentRotation, targetRotation, (float)GetProcessDeltaTime() * LeanSpeed), -LeanAngle-1, LeanAngle+1);	// -+ 1 degree of tolerance
		
		float newCamPivRot = Mathf.Wrap(Mathf.Lerp(currCamPivRot, targetCamPivRot, (float)GetProcessDeltaTime() * LeanSpeed), -(LeanAngle * .5f)-1, (LeanAngle * .5f)+1);
	
		RotationDegrees = new Godot.Vector3(RotationDegrees.X, RotationDegrees.Y, newRotation);
		CamPivNode.RotationDegrees = new Godot.Vector3(CamPivNode.RotationDegrees.X, CamPivNode.RotationDegrees.Y, newCamPivRot);

		if (Mathf.Abs(targetRotation - currentRotation) < .05f && !(currentRotation == targetRotation)) {		//Checks to see if current rotation is < .5 degrees away from target. If so, just snap to target & return.
			RotationDegrees = new Godot.Vector3(RotationDegrees.X, RotationDegrees.Y, targetRotation);
		}

		if (Mathf.Abs(targetCamPivRot - currCamPivRot) < .05f && !(currCamPivRot == targetCamPivRot)) {		//Checks to see if current rotation is < .5 degrees away from target. If so, just snap to target & return.
			CamPivNode.RotationDegrees = new Godot.Vector3(CamPivNode.RotationDegrees.X, CamPivNode.RotationDegrees.Y, targetCamPivRot);
			//having a snap for the CamPivot breaks it, so for the time being we won't have one.
		} 

		//GD.Print($"CAM PIVOT: Current Rotation: {currCamPivRot}, Target Rotation: {targetCamPivRot}, New Rotation: {newCamPivRot}");
		//GD.Print($"Current Rotation: {currentRotation}, Target Rotation: {targetRotation}, New Rotation: {newRotation}");
		//A different way of implementing this that might work better w/ game play is Leaning char body more heavily, then rotating cam pivot inversely to a lesser degree. We maintain a small tilt while still exposing hitbox sufficiently
	}

	public void ToggleUnderWater(bool tf)
	{
		GD.PrintErr("Toggling Underwater");

		if(tf)	//if true
		{
			UnderWaterCanvasLayer.Show();
			AudioServer.SetBusEffectEnabled(0, 0, true);
		}
		
		else
		{
			UnderWaterCanvasLayer.Hide(); 
			AudioServer.SetBusEffectEnabled(0, 0, false);
		}
	}
}