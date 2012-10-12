using UnityEngine;
using System.Collections;

public class TileUnit {

    //int thisNode,l,r,u,d,lt,rt,lb,rb;
	GameObject obj;
	TileUnit l=null,r=null,u=null,d=null;
	bool movable = false;

    public void init(GameObject pObj, TileUnit pl, TileUnit pr, TileUnit pu, TileUnit pd, bool m)
    {
		obj = pObj;
		
		l = pl;
		r = pr;
		u = pu;
		d = pd;
		
		movable = m;
    }
	
	public void setVisible(bool type) {
		switch(type) {
			case false:
				obj.renderer.enabled = false;
				break;
			case true:
				obj.renderer.enabled = true;
				break;
		}
	}
	
	public void showNeighbours() {
		Debug.Log("///////Node: " + movable + "///////");
		if(l!=null) {
			Debug.Log("Left: " + l.getMovable());
		}
		if(r!=null) {
			Debug.Log("Right: " + r.getMovable());
		}
		if(u!=null) {
			Debug.Log("Up: " + u.getMovable());
		}
		if(d!=null) {
			Debug.Log("Down: " + d.getMovable());
		}
		Debug.Log("///////Node: " + movable + "///////");
	}
	
	public bool getMovable() {
		return movable;
	}
	
	public void setTile(string type, TileUnit tile) {
		switch(type) {
			case "left":
				l = tile;
				break;
			case "right":
				r = tile;
				break;
			case "top":
				u = tile;
				break;
			case "bottom":
				d = tile;
				break;
		}
		//Debug.Log("YUP!");
	}
	
	public GameObject getGameObject() {
		return obj;
	}
	
	public TileUnit getNeighbour(string type) {
		TileUnit temp = null;
		switch(type) {
			case "left":
				if(l!=null&&l.getMovable()) {
					temp = l;
				}
				break;
			case "right":
				if(r!=null&&r.getMovable()) {
					temp = r;
				}
				break;
			case "up":
				if(u!=null&&u.getMovable()) {
					temp = u;
				}
				break;
			case "down":
				if(d!=null&&d.getMovable()) {
					temp = d;
				}
				break;
		}
		
		return temp;
	}
}

public class Tilemap : MonoBehaviour {
	//public Hashtable nodeList = new Hashtable();
	public ArrayList greenTiles = new ArrayList();
	public ArrayList redTiles = new ArrayList();
	
	public ArrayList pathList = new ArrayList();
	public ArrayList closedList = new ArrayList();
	
	public TileUnit[,] tileNodes = new TileUnit[tileRowCount,tileColCount];
	public const int tileRowCount = 24;	//30
	public const int tileColCount = 58;	//20
	public int[,] tileData = new int[tileRowCount, tileColCount] {
		{ 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
		{ 0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
		{ 0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }, 
		{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
	};
	void setTileNeighbour(TileUnit thisTile, int rowNum, int colNum) {
		//Debug.Log("setTileNeighbour: " + rowNum + " " + colNum);
		//[24][58]
		
		//Debug.Log(rowNum + " " + colNum);
		
		if(colNum>0) {	//Check Left Tile
			TileUnit t1 = tileNodes[rowNum,colNum-1] as TileUnit;
			
			//Debug.Log("t1: " + t1.getMovable());
			thisTile.setTile("left",t1);
			t1.setTile("right",thisTile);
		}
		
		if(rowNum>0) {	//Check Top Tile
			TileUnit t2 = tileNodes[rowNum-1,colNum] as TileUnit;
			
			//Debug.Log("t2: " + t2.getMovable());
			thisTile.setTile("top",t2);
			t2.setTile("bottom",thisTile);
		}
	}
	
	TileUnit genNode(string type, Vector3 pos) {
		TileUnit tu = new TileUnit();
		
		GameObject t = null;
		switch(type) {
			case "red":
				t = Instantiate (Resources.Load("RedCube"), pos ,Quaternion.identity) as GameObject;
				tu.init(t,null,null,null,null,false);
				break;
			case "green":
				t = Instantiate (Resources.Load("GreenCube"), pos ,Quaternion.identity) as GameObject;
				tu.init(t,null,null,null,null,true);
				break;
		}
		
		switch(type) {
			case "green":
				greenTiles.Add(tu);
				break;
			case "red":
				redTiles.Add(tu);
				break;
		}
		return tu;
	}
	
	GameObject genPlayer(Vector3 pos) {	
		return Instantiate (Resources.Load("Player"), pos ,Quaternion.identity) as GameObject;
	}
	
	void genGrid() {
		TileUnit tempNode;	
		
		float startPosX = 5;	//10
		float startPosY = 5;	//10
		
		float endPosX = Camera.mainCamera.pixelWidth-10;
		float endPosY = Camera.mainCamera.pixelHeight-10;
		
		string cube;
		
		Vector3 v;
		v.z = 850;
		v.y = startPosY;
		
		int offset = 16;
		
		for(int i=0;i<tileRowCount;i++) {
			for(int j=0;j<tileColCount;j++) {
				v.x = startPosX + j*offset;
				v.y = endPosY - i*offset;
				if(tileData[i,j]==1) {
					cube = "green";
				}
				else {
					cube = "red";
				}
				tempNode = genNode(cube,Camera.mainCamera.ScreenToWorldPoint(v));
				tileNodes[i,j] = tempNode;
				setTileNeighbour(tileNodes[i,j],i,j);
			}
		}
	}
	
	float getHeuristic(TileUnit baseTile, TileUnit tile) {
		float valX, valY;
		GameObject objBase = baseTile.getGameObject();
		GameObject obj = tile.getGameObject();
		
		valX = Mathf.Abs(objBase.transform.position.x - obj.transform.position.x);
		valY = Mathf.Abs(objBase.transform.position.y - obj.transform.position.y);
		
		return valX + valY;
	}
	
	bool tileAlreadyTraversed(TileUnit tile) {
		bool res = false;
		
		for(int i=0;i<closedList.Count;i++) {
			if(tile==closedList[i]) {
				res = true;
				break;
			}
		}
		return res;
	}
	
	TileUnit getNextNode(TileUnit start, TileUnit end) {
		float f,g,h,minF = 99999;
		TileUnit nextNode = null;
		TileUnit n = null;
		
		bool tileAlreadySearched = false;
		
		if(start!=null&&end!=null) {
			tileAlreadySearched = tileAlreadyTraversed(start.getNeighbour("right"));
			if(start.getNeighbour("right")!=null&&tileAlreadySearched==false) {
				n = start.getNeighbour("right");
				g = getHeuristic(start,n);
				h = getHeuristic(end,n);
				f = g+h*10;
				if(f<minF) {
					minF = f;
					nextNode = n;
				}
			}
			
			tileAlreadySearched = tileAlreadyTraversed(start.getNeighbour("left"));
			if(start.getNeighbour("left")!=null&&tileAlreadySearched==false) {
				n = start.getNeighbour("left");
				g = getHeuristic(start,n);
				h = getHeuristic(end,n);
				f = g+h*10;
				if(f<minF) {
					minF = f;
					nextNode = n;
				}
			}
			
			tileAlreadySearched = tileAlreadyTraversed(start.getNeighbour("top"));
			if(start.getNeighbour("up")!=null&&tileAlreadySearched==false) {
				n = start.getNeighbour("up");
				g = getHeuristic(start,n);
				h = getHeuristic(end,n);
				f = g+h*10;
				if(f<minF) {
					minF = f;
					nextNode = n;
				}
			}
			
			tileAlreadySearched = tileAlreadyTraversed(start.getNeighbour("bottom"));
			if(start.getNeighbour("down")!=null&&tileAlreadySearched==false) {
				n = start.getNeighbour("down");
				g = getHeuristic(start,n);
				h = getHeuristic(end,n);
				f = g+h*10;
				if(f<minF) {
					minF = f;
					nextNode = n;
				}
			}
		}
		return nextNode;
	}
	
	void walkPath() {
		Debug.Log("Walk Path: " + pathList.Count);
		//while(pathList.Count>0) {
			//pathList.RemoveAt(0);
		//}
		TileUnit t;
		GameObject g;
		for(int i=0;i<pathList.Count;i++) {
			t = pathList[i] as TileUnit;
			t.setVisible(false);
		}
	}
	
//	void searchNode(TileUnit tile, TileUnit end) {
//		Debug.Log("Closed Count: " + closedList.Count);
//		TileUnit nextNode = null;
//		
//		nextNode = getNextNode(tile,end);
//		if(nextNode!=null) {
//			closedList.Add(nextNode);
//			searchNode(nextNode,end);
//		}
//		walkPath();
//	}
	
	void genPath(TileUnit start, TileUnit end) {	//aStar		
		//searchNode(start,end);
		TileUnit nextNode = null;
		
		nextNode = getNextNode(start,end);
		
		if(nextNode!=null&&nextNode!=end) {
			closedList.Add(nextNode);
			pathList.Add(nextNode);
			genPath(nextNode,end);
		}
		Debug.Log("a");
		walkPath();
	}
	
	int RandomRangeExcept (int min, int max, int except) {
		int number;
		
	    do {
	
	        number = Random.Range (min, max);
	
	    } while (number == except);
	
	    return number;
	}
	
	void startAtMovableTile() {
		//GameObject[] mvTiles = GameObject.FindGameObjectsWithTag("Movable");
		int startIndex = 7;
		int endIndex = RandomRangeExcept(40,80,40);
		
		TileUnit tileStartUnit = greenTiles[startIndex] as TileUnit;
		TileUnit tileEndUnit = tileNodes[17,28];	//greenTiles[endIndex] as TileUnit;
		
		GameObject tileStart = tileStartUnit.getGameObject();
		GameObject tileEnd = tileEndUnit.getGameObject();
		
		//Place Player
		GameObject pInstance = GameObject.Find("Player");
		Vector3 v;
		v.x = tileStart.transform.position.x;
		v.y = tileStart.transform.position.y;
		v.z = tileStart.transform.position.z - 50;
		pInstance.transform.position = v;
		
		//Place CaptureFlag
		GameObject fInstance = GameObject.Find("CaptureFlag");
		Vector3 v2;
		v2.x = tileEnd.transform.position.x;
		v2.y = tileEnd.transform.position.y;
		v2.z = tileEnd.transform.position.z - 50;
		fInstance.transform.position = v2;
		
		genPath(tileStartUnit,tileEndUnit);
	}
	
	// Use this for initialization
	void Start () {
		//Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, true); //Fit whole screen

		genGrid();
		
		Debug.Log(Camera.mainCamera.pixelWidth + " " + Camera.mainCamera.pixelHeight);
		startAtMovableTile();
		Debug.Log("////////////////////");
		tileNodes[1,4].showNeighbours();
		Debug.Log("////////////////////");
//		GameObject tempNode;		
//		int countNodes = nodeList.Count;
//
//		
//		v.x = Camera.mainCamera.pixelWidth-10;
//		v.y = Camera.mainCamera.pixelHeight-10;
//		v.z = 850;
//		tempNode = genNode("green",Camera.mainCamera.ScreenToWorldPoint(v));
//		nodeList[countNodes] = tempNode;
//		
//		Debug.Log(Camera.mainCamera.pixelWidth + " and " + Camera.mainCamera.pixelHeight);
//		Debug.Log("nl: " + nodeList[0]);
	}
	
	// Update is called once per frame
	void Update () {
	}
}