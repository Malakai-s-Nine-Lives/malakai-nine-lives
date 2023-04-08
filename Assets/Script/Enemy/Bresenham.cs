using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bresenham : MonoBehaviour
{
    // for generating chances
    public static MapGrid grid;
    static Bresenham instance;
    // for converting between world values and pixels
    private static Camera camera;
    private static float z_value;

    // Start is called before the first frame update
    void Start()
    {

    }

    public static int getFour(){
        return 4;
    }
    
    void Awake()
    {
        instance = this;
        grid = GetComponent<MapGrid>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool determineActivation(int freeSightChance, int sightRadius, Vector3 enemy_position, Vector3 player_position){
        
        int x0 = (int)camera.WorldToScreenPoint(enemy_position).x;
        int y0 = (int)camera.WorldToScreenPoint(enemy_position).y;
        int x1 = (int)camera.WorldToScreenPoint(player_position).x;
        int y1 = (int)camera.WorldToScreenPoint(player_position).y;
        z_value = camera.WorldToScreenPoint(enemy_position).z;

        // Don't run Bresenham every time
        if (Random.Range(0, freeSightChance) == 0)
        {

            if ((player_position - enemy_position).magnitude > sightRadius) return false;

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
            Node node = grid.NodeFromMapPoint(camera.ScreenToWorldPoint(new Vector3(x,y,z_value)));
            if (node is not null && !node.walkable){
                // the node is not walkable
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
            Node node = grid.NodeFromMapPoint(camera.ScreenToWorldPoint(new Vector3(x,y,z_value)));
            if (node is not null && !node.walkable){
                // the node is not walkable
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
            Node node = grid.NodeFromMapPoint(vecky);

            if (node is not null && !node.walkable){
                // the node is not walkable
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
        return true;
    }
}
