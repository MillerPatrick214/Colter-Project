using System.Threading.Tasks;
using Godot;

public partial class MainMenu : Control
{
	[Export] public VBoxContainer ButtonContainer;
	[Export] public ColorRect FadeRect;
	[Export(PropertyHint.None, "suffix:s")] public float ScreenFadeTime = 2.0f;
	[Export(PropertyHint.None, "suffix:s")] public float AudioFadeTime = 2.5f;

	[ExportGroup("Continue")]
	[Export] public Button ContinueButton;

	[ExportGroup("New Game")]
	[Export] public Button NewGameButton;
	[Export] public string NewGamePath = "res://Level/FreeRoamMainMenu.tscn";

	[ExportGroup("Load Game")]
	[Export] public Button LoadGameButton;

	[ExportGroup("Settings")]
	[Export] public Button SettingsButton;
	[ExportSubgroup("Settings Submenu Buttons")]
	[Export] public VBoxContainer SettingsButtonContainer;
	[Export] public TabContainer SettingsTabContainer;
	[Export] public Button GameplayButton;
	[Export] public Button ControlsButton;
	[Export] public Button GraphicsButton;
	[Export] public Button AudioButton;
	[Export] public Button BackButton;
	[Export] public Button ApplyButton;

	[ExportGroup("Quit")]
	[Export] public Button QuitButton;
	
	
	
	public override void _Ready() { 
		FadeRect.Modulate = new Color(FadeRect.Modulate, 0.0f);
		ButtonContainer.Show();
		SettingsButtonContainer.Hide();
		SettingsTabContainer.Hide();
		ApplyButton.Hide();

		ContinueButton.Pressed += OnContinue;
		NewGameButton.Pressed += OnNewGame;
		LoadGameButton.Pressed += OnLoadGame;
		SettingsButton.Pressed += OnSettings;
		QuitButton.Pressed += OnQuit;

		ApplyButton.Pressed += OnApply;
		GameplayButton.Pressed += OnGameplay;
		ControlsButton.Pressed += OnControls;
		GraphicsButton.Pressed += OnGraphics;
		AudioButton.Pressed += OnAudio;
		BackButton.Pressed += OnBack;

		Settings.Instance.SettingChanged += ApplySettingsVisible;
		Settings.Instance.SettingUnchanged += ApplySettingsHidden;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	public async void OnContinue() {
		ContinueButton.Disabled = true; // temp
		await StartGame();
	}

	public async void OnNewGame() {
		await StartGame();
		LevelManager.Instance.GotoScene(NewGamePath);
	}

	public async void OnLoadGame() {
		ContinueButton.Disabled = true; // temp
		await StartGame();
	}

	public void OnSettings() {
		ButtonContainer.Hide();
		SettingsButtonContainer.Show();	
	}

	public void OnApply() {
		Settings.Instance.SaveSettings();
		ApplyButton.Hide();
	}

	public void OnQuit() {
		GetTree().Quit();
	}

	public void OnGameplay() {
		CheckSavePending();
		SettingsTabContainer.Show();
		SettingsTabContainer.CurrentTab = 0;
	}

	public void OnControls() {
		CheckSavePending();
		SettingsTabContainer.Show();
		SettingsTabContainer.CurrentTab = 1;
	}

	public void OnGraphics() {
		CheckSavePending();
		SettingsTabContainer.Show();
		SettingsTabContainer.CurrentTab = 2;
	}

	public void OnAudio() {
		CheckSavePending();
		SettingsTabContainer.Show();
		SettingsTabContainer.CurrentTab = 3;
	}

	public void OnBack() {
		CheckSavePending();
		SettingsButtonContainer.Hide();	
		SettingsTabContainer.Hide();
		ButtonContainer.Show();
	}

	private void ApplySettingsVisible() { if (SettingsTabContainer.Visible == true) { ApplyButton.Show(); } }
	private void ApplySettingsHidden() { ApplyButton.Hide(); }

	private void CheckSavePending() {
		if (ApplyButton.Visible == true) {
			Settings.Instance.SaveSettings();
			ApplyButton.Hide();
		}
	}

	// ############## Currently async non-functional i think ##########################
	public async Task StartGame() {
		int menuSFXBus = AudioServer.GetBusIndex("MenuSFX");
		float currVolume = AudioServer.GetBusVolumeDb(menuSFXBus);

		FadeRect.Show();
		Tween tween = GetTree().CreateTween();

		// Fade to black tween property animation
		tween.TweenProperty(FadeRect, "modulate:a", 1, ScreenFadeTime);

		// Sound fade out tween method animation
		tween.TweenMethod(Callable.From<float>(v => AudioServer.SetBusVolumeDb(menuSFXBus, v)), currVolume, -40.0f, AudioFadeTime);
		
		await ToSignal(tween, "finished");
		tween.Kill();
	}
}