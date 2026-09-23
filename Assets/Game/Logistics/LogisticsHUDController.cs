using TMPro;
using UnityEngine;
using ObsidianProtocol.Game.Logistics;

public sealed class LogisticsHUDController : MonoBehaviour
{
    private void Start()
    {
        if (LogisticsRuntime.Instance == null)
        {
            Debug.LogError("[LOGISTICS HUD] LOGISTICS RUNTIME NOT FOUND");
            return;
        }

        SetResource("MEAT", "meat", 120);
        SetResource("WOOD", "wood", 240);
        SetResource("COAL", "coal", 80);
        SetResource("IRON", "iron", 160);
        SetResource("ALLOY", "alloy", 65);
        SetResource("ELECTRONICS", "electronics", 45);
        SetResource("FUEL", "fuel", 380);

        SetStorage("FUEL", "fuel");
        SetStorage("ELECTRONICS", "electronics");

        ConnectSupplyRoutes();

        Debug.Log("[LOGISTICS HUD] RESOURCES, STORAGE AND SUPPLY CONNECTED");
    }

    private void SetResource(string rowName, string resourceId, int rate)
    {
        Transform row = transform.Find("RESOURCE OVERVIEW/" + rowName);
        if (row == null) return;

        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>(true);
        if (texts.Length > 0)
            texts[0].text = LogisticsRuntime.Instance.Resources.GetAmount(resourceId).ToString("N0");

        Transform rateTransform = row.Find("RATE");
        if (rateTransform != null)
        {
            TMP_Text rateText = rateTransform.GetComponent<TMP_Text>();
            if (rateText != null)
                rateText.text = "+" + rate.ToString("N0") + "/h";
        }
    }

    private void SetStorage(string rowName, string resourceId)
    {
        GameObject obj = GameObject.Find(rowName);
        if (obj == null) return;

        TMP_Text value = obj.GetComponentInChildren<TMP_Text>(true);
        if (value != null)
            value.text = LogisticsRuntime.Instance.Resources.GetAmount(resourceId).ToString("N0");
    }

    private void ConnectSupplyRoutes()
    {
        StrategicSupplyNetworkSystem network = LogisticsRuntime.Instance.Network;
        if (network == null)
        {
            Debug.LogError("[LOGISTICS HUD] SUPPLY NETWORK NOT FOUND");
            return;
        }

        UpdateRoute("NORTH DEPOT", "NORTH_DEPOT", "COMMAND_CENTER", "FUEL / ENERGY", network);
        UpdateRoute("IRON MINE", "IRON_MINE", "FABRICATION", "IRON / ALLOY", network);
        UpdateRoute("ELECTRONICS", "ELECTRONICS", "COMMAND_CENTER", "ELECTRONICS", network);
        UpdateRoute("FOOD STORAGE", "FOOD_STORAGE", "FIELD_FORCES", "MEAT", network);

        Debug.Log(
            "[LOGISTICS HUD] ACTIVE ROUTES: " +
            network.GetActiveLinks().Count +
            " / EFFICIENCY: " +
            network.GetNetworkEfficiency().ToString("N0") +
            "% / RISK: " +
            network.GetNetworkRisk()
        );
    }

    private void UpdateRoute(
        string rowName,
        string origin,
        string destination,
        string cargo,
        StrategicSupplyNetworkSystem network)
    {
        Transform row = transform.Find("SUPPLY/ROUTE_" + rowName);

        if (row == null)
            row = GameObject.Find("ROUTE_" + rowName)?.transform;

        if (row == null)
        {
            Debug.LogWarning("[LOGISTICS HUD] ROUTE ROW NOT FOUND: " + rowName);
            return;
        }

        StrategicSupplyLink matchedLink = null;

        foreach (StrategicSupplyLink link in network.GetLinks())
        {
            if (link.Connects(origin, destination))
            {
                matchedLink = link;
                break;
            }
        }

        if (matchedLink == null)
        {
            Debug.LogWarning("[LOGISTICS HUD] ROUTE NOT FOUND: " + origin + " -> " + destination);
            return;
        }

        SetRouteText(row, "FROM", origin.Replace("_", " "));
        SetRouteText(row, "TO", destination.Replace("_", " "));
        SetRouteText(row, "CARGO", GetDepotCargo(origin, cargo));

        float efficiency = network.GetLinkEfficiency(matchedLink);

        StrategicSupplyNode originNode;
        StrategicSupplyNode destinationNode;

        string risk = network.GetLinkRisk(matchedLink);


        SetRouteText(row, "EFF", efficiency.ToString("N0") + "%");
        SetRouteText(row, "RISK", risk);

        Debug.Log(
            "[LOGISTICS HUD] ROUTE ONLINE: " +
            origin +
            " -> " +
            destination +
            " / CAPACITY " +
            matchedLink.Capacity.ToString("N0")
        );
    }

    private string GetDepotCargo(string depotId, string fallback)
    {
        SupplyDepotSystem depots = LogisticsRuntime.Instance.Depots;
        if (depots == null)
            return fallback;

        if (!depots.TryGetDepot(depotId, out SupplyDepot depot))
            return fallback;

        if (depotId == "NORTH_DEPOT")
            return "FUEL " + depot.GetAmount(SupplyType.Fuel).ToString("N0") +
                   " / ENERGY " + depot.GetAmount(SupplyType.Energy).ToString("N0");

        if (depotId == "IRON_MINE")
            return "MATERIALS " + depot.GetAmount(SupplyType.FabricationMaterials).ToString("N0");

        if (depotId == "ELECTRONICS")
            return "RESOURCES " + depot.GetAmount(SupplyType.Resources).ToString("N0");

        if (depotId == "FOOD_STORAGE")
            return "RESOURCES " + depot.GetAmount(SupplyType.Resources).ToString("N0");

        return fallback;
    }
    private void SetRouteText(Transform row, string objectName, string value)
    {
        Transform target = row.Find(objectName);

        if (target == null)
            return;

        TMP_Text text = target.GetComponent<TMP_Text>();

        if (text != null)
            text.text = value;
    }
}






