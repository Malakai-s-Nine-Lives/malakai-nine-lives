using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bresenham : MonoBehaviour
{
    // for generating chances
    // private Random rnd = new Random();
    MapGrid grid;
    AStar aStar;  // To run A*
    static Bresenham instance;

    static Collider2D groundColliders;
    static Collider2D malakaiColliders;

    private static Camera camera;

    
    public static Vector2 gridWorldSize;  // The size of our world map
    static int gridSizeX, gridSizeY;  // Grid sizes
    static public float nodeRadius; // The radius of each cell (Node) in the grid
    static float nodeDiameter;  // Diameter of each cell (Node) in the grid

    private static float z_value;


    // Start is called before the first frame update
    void Start()
    {
        // grid = GetComponent<MapGrid>();

        
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        aStar = GetComponent<AStar>();
        // groundColliders = GetComponent<Collider2D>();
        // grid = GetComponent<MapGrid>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool determineActivation(int freeSightChance, Vector3 enemy_position, Vector3 player_position){

        
        int x0 = (int)camera.WorldToScreenPoint(enemy_position).x;
        int y0 = (int)camera.WorldToScreenPoint(enemy_position).y;
        int x1 = (int)camera.WorldToScreenPoint(player_position).x;
        int y1 = (int)camera.WorldToScreenPoint(player_position).y;
        z_value = camera.WorldToScreenPoint(enemy_position).z;

        // Don't run Bresenham every time
        if (Random.Range(0, freeSightChance) == 0)
        {
            if (Mathf.Abs(y1-y0) < Mathf.Abs(x1-x0)) {
                if (x0 > x1) {
                    return plotLineLow(x1, y1, x0, y0);
                } else {
                    return plotLineLow(x0, y0, x1, y1);
                }
            } else {
                if (y0 > y1){
                    return plotLineHigh(x1, y1, x0, y0);
                } else {
                    return plotLineHigh(x0, y0, x1, y1);
                }
            }
        }

        // if ((Random.Range(0, freeSightChance) == 0) && FreeSightChecker(enemy_position, player_position)){
        //     // Debug.Log("START whole");
        //     // Debug.Log("enemy: " + enemy_position);
        //     // Debug.Log("player: " + player_position);
            
            
        //     // Vector2 vecky = new Vector2(1,1);
        //     // Node node = instance.aStar.grid.NodeFromMapPoint(enemy_position);
        //     // Debug.Log(node);
        //     // Debug.Break();
            
        //     return true;
        // }

        return false;
    }

    private static bool plotLineLow(int x0, int y0, int x1, int y1){
        int dx = x1 - x0;
        int dy = y1 - y0;
        int yi = 1;
        if (dy < 0){
            yi = -1;
            dy = -dy;
        }
        int D = (2*dy) - dx;
        int y = y0;
        for (int x = x0; x <= x1; x++){
            // This is the same as the plot(x,y) step in the Bresenham pseudocode
            Node node = instance.aStar.grid.NodeFromMapPoint(camera.ScreenToWorldPoint(new Vector3(x,y,z_value)));
            if (node is not null && !node.walkable){
                // the node is not walkable
                // Debug.Log(node);
                Debug.Log("Malakai is Gone!");
                return false;
            }

            if (D>0){
                y = y + yi;
                D = D + (2 * (dy - dx));
            } else {
                D = D + 2*dy;
            }
        }
        // if there were no ground collisions
        // there is a line of sight to Malakai
        Debug.Log("I can see Malakai!!!");
        return true;
    }

    private static bool plotLineHigh(int x0, int y0, int x1, int y1){
        int dx = x1 - x0;
        int dy = y1 - y0;
        int xi = 1;
        if (dx < 0){
            xi = -1;
            dx = -dx;
        }
        int D = (2*dx) - dy;
        int x = x0;
        for (int y = y0; y <= y1; y++){
            // This is the same as the plot(x,y) step in the Bresenham pseudocode
            Node node = instance.aStar.grid.NodeFromMapPoint(camera.ScreenToWorldPoint(new Vector3(x,y,z_value)));
            if (node is not null && !node.walkable){
                // the node is not walkable
                // Debug.Log(node);
                Debug.Log("Malakai is Gone!");
                return false;
            }

            if (D>0){
                x = x + xi;
                D = D + (2 * (dx - dy));
            } else {
                D = D + 2*dx;
            }
        }
        // if there were no ground collisions
        // there is a line of sight to Malakai
        Debug.Log("I can see Malakai!!!");
        return true;
    }


    private static bool FreeSightChecker(Vector3 enemy_position, Vector3 player_position){

        int x1 = (int)camera.WorldToScreenPoint(player_position).x;
        int y1 = (int)camera.WorldToScreenPoint(player_position).y;
        int x0 = (int)camera.WorldToScreenPoint(enemy_position).x;
        int y0 = (int)camera.WorldToScreenPoint(enemy_position).y;
        float z = camera.WorldToScreenPoint(enemy_position).z;
        
        int dx = x1 - x0;
        int dy = y1 - y0;
        int D = 2*dy - dx;
        int y = y0;

        Debug.Log("START parts");
        for (int x = x0; x < x1; x++) 
        {
            Vector2 vecky = (Vector2) camera.ScreenToWorldPoint(new Vector3(x,y,z));
            Node node = instance.aStar.grid.NodeFromMapPoint(vecky);
            // Debug.Log(node); Debug.Break();

            if (node is not null && !node.walkable){
                // the node is not walkable
                Debug.Log(node);
                Debug.Log("Malakai is Gone!!!");
                // Debug.DrawRay(enemy_position, player_position, Color.green);
                return false;
            }

            if (D > 0)
            {
                y = y + 1;
                D = D - 2*dx;
            }
            D = D + 2*dy;
        }
        
        // if there were no ground collisions
        // there is a line of sight to Malakai
        Debug.Log("I can see Malakai!!!");
        return true;
    }
}
