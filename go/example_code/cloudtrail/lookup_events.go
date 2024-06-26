// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0
// snippet-start:[cloudtrail.go.lookup_events.complete]
package main

// snippet-start:[cloudtrail.go.lookup_events.imports]
import (
	"github.com/aws/aws-sdk-go/aws"
	"github.com/aws/aws-sdk-go/aws/session"
	"github.com/aws/aws-sdk-go/service/cloudtrail"

	"flag"
	"fmt"
	"time"
)

// snippet-end:[cloudtrail.go.lookup_events.imports]

func main() {
	// Trail name required
	// snippet-start:[cloudtrail.go.lookup_events.vars]
	trailNamePtr := flag.String("n", "", "The name of the trail")

	flag.Parse()

	if *trailNamePtr == "" {
		fmt.Println("You must supply a trail name")
		return
	}
	// snippet-end:[cloudtrail.go.lookup_events.vars]

	// snippet-start:[cloudtrail.go.lookup_events.session]
	sess := session.Must(session.NewSessionWithOptions(session.Options{
		SharedConfigState: session.SharedConfigEnable,
	}))
	// snippet-end:[cloudtrail.go.lookup_events.session]

	// snippet-start:[cloudtrail.go.lookup_events.lookup]
	svc := cloudtrail.New(sess)

	input := &cloudtrail.LookupEventsInput{EndTime: aws.Time(time.Now())}

	resp, err := svc.LookupEvents(input)
	if err != nil {
		fmt.Println("Got error calling CreateTrail:")
		fmt.Println(err.Error())
		return
	}

	fmt.Println("Found", len(resp.Events), "events before now")
	fmt.Println("")

	for _, event := range resp.Events {
		fmt.Println("Event:")
		fmt.Println(aws.StringValue(event.CloudTrailEvent))
		fmt.Println("")
		fmt.Println("Name    ", aws.StringValue(event.EventName))
		fmt.Println("ID:     ", aws.StringValue(event.EventId))
		fmt.Println("Time:   ", aws.TimeValue(event.EventTime))
		fmt.Println("User:   ", aws.StringValue(event.Username))

		fmt.Println("Resources:")

		for _, resource := range event.Resources {
			fmt.Println("  Name:", aws.StringValue(resource.ResourceName))
			fmt.Println("  Type:", aws.StringValue(resource.ResourceType))
		}

		fmt.Println("")
		// snippet-end:[cloudtrail.go.lookup_events.lookup]
	}
}

// snippet-end:[cloudtrail.go.lookup_events.complete]
