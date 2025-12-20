using UnityEngine;

/// <summary>
/// Centralized constants for the game to avoid magic numbers
/// </summary>
public static class GameConstants
{
    // === TIMING CONSTANTS ===

    /// <summary>
    /// Delay before transitioning to next scene after dish completion (seconds)
    /// </summary>
    public const float DISH_COMPLETE_DELAY = 2f;

    /// <summary>
    /// Timer warning threshold (seconds remaining)
    /// </summary>
    public const float TIMER_WARNING_THRESHOLD = 30f;

    /// <summary>
    /// Jumpscare image display duration (seconds)
    /// </summary>
    public const float JUMPSCARE_IMAGE_DURATION = 2f;

    /// <summary>
    /// Jumpscare sound delay (seconds)
    /// </summary>
    public const float JUMPSCARE_SOUND_DELAY = 0.5f;

    /// <summary>
    /// Jumpscare scene transition delay (seconds)
    /// </summary>
    public const float JUMPSCARE_SCENE_TRANSITION = 9.5f;

    /// <summary>
    /// Duration to show final dish before scene end (seconds)
    /// </summary>
    public const float FINAL_DISH_DISPLAY_DURATION = 1.5f;


    // === UI SCALE CONSTANTS ===

    /// <summary>
    /// Scale factor for ingredients placed on plate
    /// </summary>
    public const float INGREDIENT_ON_PLATE_SCALE = 0.2f;

    /// <summary>
    /// Default scale for UI elements (x, y, z)
    /// </summary>
    public static readonly Vector3 DEFAULT_UI_SCALE = new Vector3(1f, 1f, 1f);


    // === POSITIONING CONSTANTS ===

    /// <summary>
    /// Random offset range for ingredient X position on plate
    /// </summary>
    public const float INGREDIENT_OFFSET_X = 50f;

    /// <summary>
    /// Random offset range for ingredient Y position on plate
    /// </summary>
    public const float INGREDIENT_OFFSET_Y = 30f;

    /// <summary>
    /// Character left position (off-screen)
    /// </summary>
    public const float CHARACTER_LEFT_POSITION = -5f;

    /// <summary>
    /// Character center position (on-screen)
    /// </summary>
    public const float CHARACTER_CENTER_POSITION = 0f;

    /// <summary>
    /// Character right position (off-screen)
    /// </summary>
    public const float CHARACTER_RIGHT_POSITION = 5f;


    // === SIZE CONSTANTS ===

    /// <summary>
    /// Final dish image size on plate
    /// </summary>
    public static readonly Vector2 DISH_SIZE = new Vector2(400f, 400f);

    /// <summary>
    /// Vertical offset for dish position on plate
    /// </summary>
    public const float DISH_Y_OFFSET = 120f;


    // === SCORE CONSTANTS ===

    /// <summary>
    /// Score multiplier for remaining time (points per second)
    /// </summary>
    public const int TIME_BONUS_MULTIPLIER = 10;

    /// <summary>
    /// Number of digits for formatted score display
    /// </summary>
    public const int SCORE_DISPLAY_DIGITS = 6;


    // === TIME FORMATTING CONSTANTS ===

    /// <summary>
    /// Seconds in a minute
    /// </summary>
    public const int SECONDS_PER_MINUTE = 60;

    /// <summary>
    /// Minimum time value for warnings
    /// </summary>
    public const int MINIMUM_WARNING_TIME = 2;


    // === SETTINGS CONSTANTS ===

    /// <summary>
    /// Language index for Turkish
    /// </summary>
    public const int LANGUAGE_TURKISH = 0;

    /// <summary>
    /// Language index for English
    /// </summary>
    public const int LANGUAGE_ENGLISH = 1;

    /// <summary>
    /// Sound enabled state
    /// </summary>
    public const float SOUND_ON = 1f;

    /// <summary>
    /// Sound disabled state
    /// </summary>
    public const float SOUND_OFF = 0f;


    // === CURSOR CONSTANTS ===

    /// <summary>
    /// Path to default cursor texture (used in all scenes except table01)
    /// </summary>
    public const string DEFAULT_CURSOR_PATH = "Images/other/cursors/cursor";

    /// <summary>
    /// Path to pointer cursor texture (used in table01 scene)
    /// </summary>
    public const string POINTER_CURSOR_PATH = "Images/other/cursors/pointer";

    /// <summary>
    /// Default cursor hotspot X position (the click point)
    /// For 32x32 cursor, (0,0) is top-left corner
    /// </summary>
    public const float CURSOR_HOTSPOT_X = 1f;

    /// <summary>
    /// Default cursor hotspot Y position (the click point)
    /// For 32x32 cursor, (0,0) is top-left corner
    /// </summary>
    public const float CURSOR_HOTSPOT_Y = 1f;

    /// <summary>
    /// Pointer cursor hotspot X position (for table01 scene)
    /// For 16x16 pointer hand cursor - finger tip position
    /// </summary>
    public const float POINTER_HOTSPOT_X = 7f;

    /// <summary>
    /// Pointer cursor hotspot Y position (for table01 scene)
    /// For 16x16 pointer hand cursor - finger tip position
    /// </summary>
    public const float POINTER_HOTSPOT_Y = 0f;

    /// <summary>
    /// Default cursor hotspot as Vector2
    /// </summary>
    public static readonly Vector2 CURSOR_HOTSPOT = new Vector2(CURSOR_HOTSPOT_X, CURSOR_HOTSPOT_Y);

    /// <summary>
    /// Pointer cursor hotspot as Vector2
    /// </summary>
    public static readonly Vector2 POINTER_HOTSPOT = new Vector2(POINTER_HOTSPOT_X, POINTER_HOTSPOT_Y);

    /// <summary>
    /// Scene name for table scene
    /// </summary>
    public const string SCENE_TABLE01 = "table01";
}
